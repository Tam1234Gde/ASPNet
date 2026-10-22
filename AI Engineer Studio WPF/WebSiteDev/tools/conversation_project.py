"""Bounded project inspection. No shell, source writes or persistent content cache."""
import os, re, json, fnmatch
from pathlib import Path
from collections import OrderedDict

EXCLUDED = {'.git', '.vs', '.idea', 'bin', 'obj', 'node_modules', 'artifacts', 'backups', '__pycache__'}
SENSITIVE = {'.env', 'settings.json', 'appsettings.json', 'id_rsa', 'id_ed25519', '.npmrc', '.pypirc'}

def redact(text):
    text = re.sub(r'sk-[A-Za-z0-9_-]{12,}', '[REDACTED]', text)
    return re.sub(r'(?im)((?:password|api[_-]?key|token|secret)\s*[\"\x27]?\s*[:=]\s*)[^\r\n,]+', r'\1[REDACTED]', text)

class ProjectReader:
    def __init__(self, root, rpc):
        self.root = Path(root).resolve()
        self.rpc = rpc
        self.cache = OrderedDict()

    def path(self, value):
        p = self.root / value
        resolved = p.resolve()
        if resolved != self.root and self.root not in resolved.parents:
            raise ValueError('Путь вне проекта')
        for part in p.relative_to(self.root).parts:
            if part.lower() in EXCLUDED or part.lower() in SENSITIVE or part.lower().startswith('.env.'):
                raise ValueError('Служебный или секретный файл недоступен')
        if resolved.suffix.lower() in {'.pem', '.key', '.pfx', '.p12'}:
            raise ValueError('Секретный файл недоступен')
        current = p
        while current != self.root:
            if current.is_symlink() or (hasattr(current, 'is_junction') and current.is_junction()):
                raise ValueError('Ссылки недоступны')
            current = current.parent
        return resolved

    def files(self, directory='.'):
        root = self.path(directory)
        count = 0
        for folder, dirs, names in os.walk(root, followlinks=False):
            dirs[:] = [d for d in dirs if d.lower() not in EXCLUDED and not Path(folder, d).is_symlink() and not (hasattr(Path(folder,d), 'is_junction') and Path(folder,d).is_junction())]
            for name in names:
                try: p = self.path(str(Path(folder, name).relative_to(self.root)))
                except ValueError: continue
                yield p
                count += 1
                if count >= 10000: return

    def text(self, p):
        stat = p.stat()
        if stat.st_size > 512000: raise ValueError('Файл больше 512 КБ: выберите меньший файл')
        stamp = (stat.st_mtime_ns, stat.st_size)
        entry = self.cache.get(p)
        if entry and entry[0] == stamp: return entry[1]
        data = p.read_bytes()
        if b'\0' in data: raise ValueError('Бинарный файл')
        text = redact(data.decode('utf-8', errors='replace'))
        self.cache[p] = (stamp, text)
        self.cache.move_to_end(p)
        while len(self.cache) > 16: self.cache.popitem(last=False)
        return text

    def read_file(self, filepath, start_line=1, end_line=None):
        lines = self.text(self.path(filepath)).splitlines()
        start = max(0, int(start_line or 1)-1)
        end = min(len(lines), int(end_line or start+300), start+300)
        return '\n'.join(f'{i+1}: {lines[i]}' for i in range(start,end))[:24000] + f'\n[Всего строк: {len(lines)}; показан фрагмент]'

    def get_project_tree(self, directory='.'):
        names = []
        for p in self.files(directory):
            names.append(str(p.relative_to(self.root)))
            if len(names) >= 1500: break
        return '\n'.join(names) + '\n[Не более 1500 файлов; служебные и секретные исключены]'

    def list_files(self, directory='.'):
        result = []
        for p in self.path(directory).iterdir():
            try: self.path(str(p.relative_to(self.root)))
            except ValueError: continue
            result.append(str(p.relative_to(self.root)))
            if len(result) >= 1500: break
        return '\n'.join(result)

    def search_code(self, query, file_pattern='*', case_sensitive=False):
        # Literal search avoids catastrophic user/model regex execution.
        needle = query if case_sensitive else query.casefold()
        out = []
        for p in self.files():
            if not fnmatch.fnmatch(p.name, file_pattern): continue
            try: lines = self.text(p).splitlines()
            except (ValueError, OSError): continue
            for n, line in enumerate(lines,1):
                if needle in (line if case_sensitive else line.casefold()):
                    out.append(f'{p.relative_to(self.root)}:{n}: {line[:400]}')
                    if len(out) >= 60: return '\n'.join(out) + '\n[Лимит 60 совпадений]'
        return '\n'.join(out) or 'Совпадений в доступных файлах нет.'

    def write_info_file(self, filepath, content):
        # WPF validates path again, previews and authorizes every write.
        return self.rpc('document_request', filepath=filepath, content=content)

    def get_development_status(self):
        return self.rpc('development_status')

    def tools(self):
        return {n: getattr(self,n) for n in ('read_file','list_files','get_project_tree','search_code','write_info_file','get_development_status')}
