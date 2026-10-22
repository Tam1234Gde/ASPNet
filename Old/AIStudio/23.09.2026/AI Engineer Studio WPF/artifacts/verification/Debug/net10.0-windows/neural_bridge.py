"""OpenAI-compatible agent bridge with project tools and line-delimited events."""
import json, sys, os, re, glob, shutil, subprocess, urllib.request, base64, time
try:
    import pyautogui
except ImportError:
    pyautogui = None
from pathlib import Path
if hasattr(sys.stdin, 'reconfigure'): sys.stdin.reconfigure(encoding='utf-8')
if hasattr(sys.stdout, 'reconfigure'): sys.stdout.reconfigure(encoding='utf-8')

def emit(kind, **data):
    print(json.dumps({'event': kind, **data}, ensure_ascii=False), flush=True)

def safe(base, value):
    p = (Path(value) if os.path.isabs(value) else Path(base)/value).resolve()
    root=Path(base).resolve()
    if p != root and root not in p.parents: raise ValueError('Путь выходит за пределы проекта')
    return p

def tools_for(base):
    def read_file(filepath, start_line=None, end_line=None):
        p=safe(base,filepath); lines=p.read_text(encoding='utf-8',errors='replace').splitlines(True)
        a=max(0,(start_line or 1)-1); b=end_line or len(lines)
        return ''.join(f'{i+1}: {x}' for i,x in enumerate(lines[a:b],a))
    def write_file(filepath,content):
        p=safe(base,filepath); p.parent.mkdir(parents=True,exist_ok=True); p.write_text(content,encoding='utf-8'); return f"Файл '{filepath}' сохранен"
    def edit_file_part(filepath,old_str,new_str):
        p=safe(base,filepath); text=p.read_text(encoding='utf-8',errors='replace')
        if old_str not in text: return 'Ошибка: фрагмент не найден'
        p.write_text(text.replace(old_str,new_str,1),encoding='utf-8'); return f"Файл '{filepath}' изменен"
    def list_files(directory='.'):
        return json.dumps([str(x.relative_to(base)) for x in safe(base,directory).iterdir()],ensure_ascii=False)
    def tree(directory='.'):
        return '\n'.join(str(x.relative_to(base)) for x in safe(base,directory).rglob('*') if '.git' not in x.parts and 'bin' not in x.parts and 'obj' not in x.parts)
    def search_code(query,file_pattern='*.*',case_sensitive=False):
        flags=0 if case_sensitive else re.I; rx=re.compile(query,flags); out=[]
        for p in Path(base).rglob(file_pattern):
            if not p.is_file() or any(x in p.parts for x in {'.git','bin','obj','node_modules'}): continue
            try:
                for n,line in enumerate(p.read_text(encoding='utf-8',errors='ignore').splitlines(),1):
                    if rx.search(line): out.append(f'{p.relative_to(base)}:{n}: {line.strip()}')
                    if len(out)>=100:return '\n'.join(out)
            except Exception: pass
        return '\n'.join(out) or 'Совпадений не найдено.'
    def execute_command(command,timeout=60):
        r=subprocess.run(command,shell=True,cwd=base,capture_output=True,text=True,timeout=min(int(timeout),300)); return f'Код завершения: {r.returncode}\n{r.stdout}\n{r.stderr}'
    def create_folder(folder_path): safe(base,folder_path).mkdir(parents=True,exist_ok=True); return 'Папка создана'
    def screenshot(filepath='artifacts/screenshot.png'):
        if pyautogui is None: return 'Ошибка: установите pyautogui для скриншотов'
        p=safe(base,filepath); p.parent.mkdir(parents=True,exist_ok=True); pyautogui.screenshot().save(str(p)); return f'Скриншот сохранён: {p.relative_to(base)}'
    def mouse_position():
        if pyautogui is None: return 'Ошибка: установите pyautogui'
        return str(pyautogui.position())
    def move_mouse(x,y):
        if not req.get('computer_control_enabled',False): raise PermissionError('Управление компьютером отключено пользователем')
        if pyautogui is None: raise RuntimeError('Установите pyautogui')
        pyautogui.moveTo(int(x),int(y)); return f'Курсор перемещён в ({x},{y})'
    def mouse_click(button='left'):
        if not req.get('computer_control_enabled',False): raise PermissionError('Управление компьютером отключено пользователем')
        if pyautogui is None: raise RuntimeError('Установите pyautogui')
        pyautogui.click(button=button); return f'Выполнен клик: {button}'
    return {'read_file':read_file,'write_file':write_file,'edit_file_part':edit_file_part,'list_files':list_files,'get_project_tree':tree,'search_code':search_code,'execute_command':execute_command,'create_folder':create_folder,'screenshot':screenshot,'mouse_position':mouse_position,'move_mouse':move_mouse,'mouse_click':mouse_click}

SCHEMAS = [
    ('list_files', 'Показать файлы', {'directory': {'type': 'string'}}),
    ('read_file', 'Прочитать файл', {'filepath': {'type': 'string'}, 'start_line': {'type': 'integer'}, 'end_line': {'type': 'integer'}}),
    ('write_file', 'Создать или перезаписать файл', {'filepath': {'type': 'string'}, 'content': {'type': 'string'}}),
    ('edit_file_part', 'Точечно заменить фрагмент файла', {'filepath': {'type': 'string'}, 'old_str': {'type': 'string'}, 'new_str': {'type': 'string'}}),
    ('create_folder', 'Создать папку', {'folder_path': {'type': 'string'}}),
    ('get_project_tree', 'Показать дерево проекта', {'directory': {'type': 'string'}}),
    ('search_code', 'Поиск по коду', {'query': {'type': 'string'}, 'file_pattern': {'type': 'string'}, 'case_sensitive': {'type': 'boolean'}}),
    ('execute_command', 'Выполнить команду в проекте', {'command': {'type': 'string'}, 'timeout': {'type': 'integer'}}),
    ('screenshot', 'Сделать скриншот рабочего стола и сохранить в проект', {'filepath': {'type': 'string'}}),
    ('mouse_position', 'Получить координаты курсора', {}),
    ('move_mouse', 'Переместить курсор', {'x': {'type': 'integer'}, 'y': {'type': 'integer'}}),
    ('mouse_click', 'Нажать кнопку мыши', {'button': {'type': 'string', 'enum': ['left', 'right']}}),
]

def main():
    req=json.load(sys.stdin); base=os.path.abspath(req.get('project_path') or os.getcwd()); key=req.get('api_key') or os.getenv('CVC_API_KEY')
    if not key: raise RuntimeError('Не задан API-ключ')
    from openai import OpenAI
    client=OpenAI(api_key=key,base_url=req.get('base_url') or 'https://apiduck.sytes.net/v1')
    funcs=tools_for(base); specs=[{'type':'function','function':{'name':n,'description':d,'parameters':{'type':'object','properties':p}}} for n,d,p in SCHEMAS]
    tool_catalog = '\n'.join(f'- {n}: {d}' for n,d,_ in SCHEMAS)
    system=req.get('system_prompt','Ты — автономный senior-разработчик.')+f"\nРабочая папка: {base}\nИзменяй проект инструментами, проверяй результат. Перед удалением не действуй без явного указания пользователя.\n\nДоступные методы управления проектом (каталог обновляется из SCHEMAS при каждом запуске):\n{tool_catalog}\n\nМетоды приложения: используй инструменты проекта для чтения и изменения исходников; управление курсором разрешено только при включённом пользователем computer_control_enabled."
    messages=[{'role':'system','content':system}]+req.get('history',[])
    # Без искусственного ограничения числа шагов: цикл завершается финальным ответом модели.
    while True:
        r=client.chat.completions.create(model=req.get('model','gpt-5.6-terra'),messages=messages,tools=specs,tool_choice='auto',temperature=float(req.get('temperature',.2)),max_tokens=int(req.get('max_tokens',8192)))
        m=r.choices[0].message
        calls=getattr(m,'tool_calls',None)
        if calls:
            messages.append({'role':'assistant','content':m.content or '', 'tool_calls':[{'id':c.id,'type':'function','function':{'name':c.function.name,'arguments':c.function.arguments}} for c in calls]})
            for c in calls:
                name=c.function.name
                try: args=json.loads(c.function.arguments or '{}'); emit('tool_start',name=name,args=args); result=str(funcs[name](**args)); emit('tool_end',name=name,result=result[:500])
                except Exception as e: result='Ошибка: '+str(e); emit('tool_end',name=name,result=result)
                messages.append({'role':'tool','tool_call_id':c.id,'content':result})
            continue
        emit('final',content=m.content or ''); return
    raise RuntimeError('Превышен лимит шагов агента')
try: main()
except Exception as e: print(json.dumps({'ok':False,'error':str(e)},ensure_ascii=False),flush=True); sys.exit(1)
