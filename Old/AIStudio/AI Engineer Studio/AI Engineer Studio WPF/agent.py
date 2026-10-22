import os
import json
import base64
import io
import shutil
import subprocess
import glob
import re
import urllib.request
import urllib.parse
import platform
from typing import Optional, Dict, Any, List
from openai import OpenAI

# ==========================================
# КОНФИГУРАЦИЯ APIDUCK И ОПИСАНИЕ МОДЕЛЕЙ
# ==========================================
API_KEY = os.getenv("CVC_API_KEY", "sk-cvc-1b6c31ea59c38507badec70ead406011503a560a9a8c76ec373cfc0ddb58587e")
BASE_URL = "https://apiduck.sytes.net/v1"

client = OpenAI(api_key=API_KEY, base_url=BASE_URL)

AVAILABLE_MODELS = {
    "1": {
        "id": "gpt-5.6-terra",
        "name": "OpenAI · GPT 5.6 Terra",
        "desc": "Универсальный флагман. Отлично решает сложные логические задачи, пишет чистый код и поддерживает Function Calling."
    },
    "2": {
        "id": "gpt-5.6-luna",
        "name": "OpenAI · GPT 5.6 Luna",
        "desc": "Быстрая и облегченная модель. Подходит для простых запросов, генерации текстов и быстрой правки файлов."
    },
    "3": {
        "id": "claude-sonnet-4-6",
        "name": "Anthropic · Claude Sonnet 4.6",
        "desc": "Эталон для написания и рефакторинга кода, работы с тяжелой архитектурой и сложного технического анализа."
    },
    "4": {
        "id": "claude-opus-4-6",
        "name": "Anthropic · Claude Opus 4.6",
        "desc": "Максимальный уровень глубокого рассуждения (Reasoning), написания лонгридов, документации и работы с большими контекстами."
    },
    "5": {
        "id": "deepseek-v4-flash",
        "name": "DeepSeek · V4 Flash",
        "desc": "Сверхбыстрая генерация кода и ответов. Идеально для экспресс-отладки и коротких вопросов."
    },
    "6": {
        "id": "deepseek-v4-pro",
        "name": "DeepSeek · V4 Pro",
        "desc": "Мощная модель для точных математических расчетов, системного программирования и работы с алгоритмами."
    },
    "7": {
        "id": "grok-4.5",
        "name": "xAI · Grok 4.5",
        "desc": "Высокая скорость, креативность, сильна в логических тестах и нестандартных инженерных решениях."
    },
    "8": {
        "id": "qwen3.7-max",
        "name": "Alibaba · Qwen 3.7 Max",
        "desc": "Отличная работа с многоязычным контентом, глубоким анализом структурированных данных и JSON."
    },
    "9": {
        "id": "kimi-k3",
        "name": "Moonshot · Kimi K3",
        "desc": "Специалист по обработке и анализу гигантских текстовых объемов и длинных файловых контекстов."
    }
}

# ==========================================
# БАЗОВЫЕ ФУНКЦИИ РАБОТЫ С ПУТЯМИ
# ==========================================
def resolve_path(base_dir: str, path: str) -> str:
    """Преобразует относительный путь относительно директории активного проекта."""
    if os.path.isabs(path):
        return path
    return os.path.normpath(os.path.join(base_dir, path))

# ==========================================
# ИНСТРУМЕНТЫ УПРАВЛЕНИЯ ФАЙЛАМИ И ПАПКАМИ ПРОЕКТА
# ==========================================
def list_files(base_dir: str, directory: str = ".") -> str:
    """Возвращает список файлов и папок в директории проекта."""
    try:
        target = resolve_path(base_dir, directory)
        files = os.listdir(target)
        return json.dumps({"directory": target, "items": files}, ensure_ascii=False)
    except Exception as e:
        return f"Ошибка при чтении директории: {e}"

def read_file(base_dir: str, filepath: str, start_line: Optional[int] = None, end_line: Optional[int] = None) -> str:
    """Читает и возвращает содержимое файла (полностью или указанный диапазон строк)."""
    try:
        target = resolve_path(base_dir, filepath)
        with open(target, "r", encoding="utf-8", errors="replace") as f:
            if start_line is not None or end_line is not None:
                lines = f.readlines()
                start = max(0, (start_line - 1) if start_line else 0)
                end = end_line if end_line else len(lines)
                selected = lines[start:end]
                numbered = [f"{start + idx + 1}: {line}" for idx, line in enumerate(selected)]
                return "".join(numbered)
            return f.read()
    except Exception as e:
        return f"Ошибка при чтении файла {filepath}: {e}"

def write_file(base_dir: str, filepath: str, content: str) -> str:
    """Создает или перезаписывает файл в проекте."""
    try:
        target = resolve_path(base_dir, filepath)
        folder = os.path.dirname(target)
        if folder:
            os.makedirs(folder, exist_ok=True)
        with open(target, "w", encoding="utf-8") as f:
            f.write(content)
        return f"Файл '{filepath}' успешно сохранен."
    except Exception as e:
        return f"Ошибка при записи в файл {filepath}: {e}"

def edit_file_part(base_dir: str, filepath: str, old_str: str, new_str: str) -> str:
    """Заменяет указанный фрагмент текста в файле на новый без перезаписи всего файла."""
    try:
        target = resolve_path(base_dir, filepath)
        if not os.path.exists(target):
            return f"Ошибка: файл '{filepath}' не найден."
        with open(target, "r", encoding="utf-8", errors="replace") as f:
            content = f.read()
        if old_str not in content:
            return f"Ошибка: искомый фрагмент 'old_str' не найден в файле '{filepath}'."
        count = content.count(old_str)
        content = content.replace(old_str, new_str, 1)
        with open(target, "w", encoding="utf-8") as f:
            f.write(content)
        suffix = f" (найдено совпадений: {count}, заменено первое)" if count > 1 else ""
        return f"Фрагмент в файле '{filepath}' успешно заменен.{suffix}"
    except Exception as e:
        return f"Ошибка при точечной правке файла {filepath}: {e}"

def append_to_file(base_dir: str, filepath: str, content: str) -> str:
    """Дописывает содержимое в конец существующего или нового файла."""
    try:
        target = resolve_path(base_dir, filepath)
        folder = os.path.dirname(target)
        if folder:
            os.makedirs(folder, exist_ok=True)
        with open(target, "a", encoding="utf-8") as f:
            f.write(content)
        return f"Контент успешно добавлен в конец файла '{filepath}'."
    except Exception as e:
        return f"Ошибка при добавлении в файл {filepath}: {e}"

def create_folder(base_dir: str, folder_path: str) -> str:
    """Создает новую папку в проекте."""
    try:
        target = resolve_path(base_dir, folder_path)
        os.makedirs(target, exist_ok=True)
        return f"Папка '{folder_path}' успешно создана."
    except Exception as e:
        return f"Ошибка при создании папки {folder_path}: {e}"

def delete_item(base_dir: str, path: str) -> str:
    """Удаляет файл или папку со всем содержимым в проекте."""
    try:
        target = resolve_path(base_dir, path)
        if not os.path.exists(target):
            return f"Путь '{path}' не существует."
        if os.path.isdir(target):
            shutil.rmtree(target)
            return f"Папка '{path}' и всё её содержимое удалены."
        else:
            os.remove(target)
            return f"Файл '{path}' удален."
    except Exception as e:
        return f"Ошибка при удалении {path}: {e}"

def move_item(base_dir: str, source_path: str, destination_path: str) -> str:
    """Перемещает или переименовывает файл/папку внутри проекта."""
    try:
        src = resolve_path(base_dir, source_path)
        dest = resolve_path(base_dir, destination_path)
        dest_folder = os.path.dirname(dest)
        if dest_folder:
            os.makedirs(dest_folder, exist_ok=True)
        shutil.move(src, dest)
        return f"Объект перемещен из '{source_path}' в '{destination_path}'."
    except Exception as e:
        return f"Ошибка при перемещении {source_path}: {e}"

def get_project_tree(base_dir: str, directory: str = ".", ignore_dirs: Optional[list] = None) -> str:
    """Возвращает дерево файлов и папок проекта."""
    if ignore_dirs is None:
        ignore_dirs = [".git", "venv", "__pycache__", ".idea", ".vscode", "node_modules", ".env"]
    
    target_root = resolve_path(base_dir, directory)
    tree_lines = []
    try:
        for root, dirs, files in os.walk(target_root):
            dirs[:] = [d for d in dirs if d not in ignore_dirs]
            rel_path = os.path.relpath(root, target_root)
            depth = 0 if rel_path == "." else rel_path.count(os.sep) + 1
            indent = "  " * depth
            folder_name = os.path.basename(root) if rel_path != "." else os.path.basename(target_root)
            tree_lines.append(f"{indent}📁 {folder_name}/")
            sub_indent = "  " * (depth + 1)
            for f in files:
                tree_lines.append(f"{sub_indent}📄 {f}")
        return "\n".join(tree_lines)
    except Exception as e:
        return f"Ошибка при сканировании структуры проекта: {e}"

# ==========================================
# НОВЫЕ ИНСТРУМЕНТЫ: ПОИСК ПО КОДУ И ФАЙЛАМ
# ==========================================
def search_code(base_dir: str, query: str, file_pattern: str = "*.*", case_sensitive: bool = False) -> str:
    """Ищет вхождения строки или регулярного выражения во всех файлах проекта."""
    matches = []
    flags = 0 if case_sensitive else re.IGNORECASE
    ignore_dirs = {".git", "venv", "__pycache__", ".idea", ".vscode", "node_modules"}

    try:
        regex = re.compile(query, flags)
        for root, dirs, files in os.walk(base_dir):
            dirs[:] = [d for d in dirs if d not in ignore_dirs]
            for file in files:
                if glob.fnmatch.fnmatch(file, file_pattern):
                    full_path = os.path.join(root, file)
                    rel_path = os.path.relpath(full_path, base_dir)
                    try:
                        with open(full_path, "r", encoding="utf-8", errors="ignore") as f:
                            for idx, line in enumerate(f, 1):
                                if regex.search(line):
                                    matches.append(f"{rel_path}:{idx}: {line.strip()}")
                                    if len(matches) >= 100:
                                        return "\n".join(matches) + "\n... (найдено более 100 совпадений, показаны первые 100)"
                    except Exception:
                        continue
        return "\n".join(matches) if matches else "Совпадений не найдено."
    except Exception as e:
        return f"Ошибка при поиске по коду: {e}"

def find_by_name(base_dir: str, pattern: str) -> str:
    """Ищет файлы и папки по названию или glob-шаблону (например '*.py', 'config*')."""
    matches = []
    ignore_dirs = {".git", "venv", "__pycache__", ".idea", ".vscode", "node_modules"}
    try:
        for root, dirs, files in os.walk(base_dir):
            dirs[:] = [d for d in dirs if d not in ignore_dirs]
            for item in dirs + files:
                if glob.fnmatch.fnmatch(item, pattern):
                    full_path = os.path.join(root, item)
                    rel_path = os.path.relpath(full_path, base_dir)
                    matches.append(rel_path)
        return json.dumps(matches, ensure_ascii=False, indent=2) if matches else "Файлы по данному шаблону не найдены."
    except Exception as e:
        return f"Ошибка при поиске файлов: {e}"

# ==========================================
# НОВЫЕ ИНСТРУМЕНТЫ: ТЕРМИНАЛ И СИСТЕМА
# ==========================================
def execute_command(base_dir: str, command: str, timeout: int = 60) -> str:
    """Выполняет команду shell/терминала внутри директории активного проекта."""
    try:
        process = subprocess.run(
            command,
            shell=True,
            cwd=base_dir,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            timeout=timeout
        )
        output = process.stdout
        error = process.stderr
        code = process.returncode

        res = []
        res.append(f"Код завершения: {code}")
        if output:
            res.append(f"--- STDOUT ---\n{output.strip()}")
        if error:
            res.append(f"--- STDERR ---\n{error.strip()}")
        if not output and not error:
            res.append("(Команда выполнена без вывода)")
        return "\n".join(res)
    except subprocess.TimeoutExpired:
        return f"Ошибка: Время выполнения команды превысило лимит ({timeout} сек)."
    except Exception as e:
        return f"Ошибка выполнения команды: {e}"

def get_system_info(base_dir: str) -> str:
    """Возвращает информацию о текущей ОС, окружении Python и папке проекта."""
    return json.dumps({
        "os": platform.platform(),
        "python_version": platform.python_version(),
        "architecture": platform.architecture()[0],
        "project_directory": os.path.abspath(base_dir)
    }, ensure_ascii=False, indent=2)

# ==========================================
# УПРАВЛЕНИЕ РАБОЧИМ СТОЛОМ (явное включение)
# ==========================================
# По умолчанию доступ отключен: управление ОС — привилегированная возможность.
# Включается только переменной окружения BETA_ENABLE_DESKTOP_CONTROL=1.
DESKTOP_ENABLED = os.getenv("BETA_ENABLE_DESKTOP_CONTROL", "0") == "1"
DESKTOP_CONFIRM = os.getenv("BETA_DESKTOP_CONFIRM", "1") != "0"


def _desktop():
    if not DESKTOP_ENABLED:
        raise PermissionError("Управление рабочим столом отключено. Установите BETA_ENABLE_DESKTOP_CONTROL=1 и перезапустите приложение.")
    import pyautogui
    # Перемещение мыши в левый верхний угол немедленно прерывает действие.
    pyautogui.PAUSE = 0.08
    pyautogui.FAILSAFE = True
    return pyautogui


def _confirm(action: str) -> None:
    if DESKTOP_CONFIRM:
        answer = input(f"⚠️ Разрешить действие на компьютере: {action}? [y/N] ").strip().lower()
        if answer not in {"y", "yes", "д", "да"}:
            raise PermissionError("Действие отменено пользователем.")


def get_screen_size() -> str:
    p = _desktop()
    w, h = p.size()
    return json.dumps({"width": w, "height": h})


def screenshot(base_dir: str, filepath: str = "screen.png") -> str:
    p = _desktop()
    target = resolve_path(base_dir, filepath)
    os.makedirs(os.path.dirname(target) or base_dir, exist_ok=True)
    image = p.screenshot()
    image.save(target, "PNG")
    return json.dumps({"path": filepath, "width": image.width, "height": image.height}, ensure_ascii=False)


def mouse_move(x: int, y: int, duration: float = 0.2) -> str:
    p = _desktop(); p.moveTo(x, y, duration=max(0, min(duration, 10)))
    return json.dumps({"x": x, "y": y})


def mouse_click(x: int, y: int, button: str = "left", clicks: int = 1) -> str:
    p = _desktop(); button = button.lower()
    if button not in {"left", "middle", "right"} or clicks not in {1, 2, 3}:
        raise ValueError("Допустимы button=left/middle/right и clicks=1..3")
    _confirm(f"клик {button} x={x}, y={y}, clicks={clicks}")
    p.click(x, y, clicks=clicks, button=button)
    return "Клик выполнен."


def mouse_button(button: str = "left", action: str = "down") -> str:
    p = _desktop(); button = button.lower(); action = action.lower()
    if button not in {"left", "middle", "right"} or action not in {"down", "up"}:
        raise ValueError("Допустимые значения: button left/middle/right, action down/up")
    _confirm(f"кнопка мыши {button} {action}")
    (p.mouseDown if action == "down" else p.mouseUp)(button=button)
    return "Состояние кнопки изменено."


def mouse_scroll(amount: int, x: Optional[int] = None, y: Optional[int] = None) -> str:
    p = _desktop()
    if x is not None and y is not None: p.moveTo(x, y)
    p.scroll(max(-100, min(amount, 100)))
    return "Прокрутка выполнена."


def keyboard_type(text: str, interval: float = 0.02) -> str:
    p = _desktop(); _confirm(f"ввод текста длиной {len(text)} символов")
    p.write(text, interval=max(0, min(interval, 1)))
    return "Текст введён."


def keyboard_hotkey(keys: List[str]) -> str:
    p = _desktop()
    if not keys or len(keys) > 6: raise ValueError("Нужно от 1 до 6 клавиш")
    _confirm("горячая клавиша: " + "+".join(keys))
    p.hotkey(*keys)
    return "Горячая клавиша выполнена."

# ==========================================
# НОВЫЕ ИНСТРУМЕНТЫ: СЕТЬ И ВЕБ
# ==========================================
def fetch_web_page(url: str, max_chars: int = 8000) -> str:
    """Загружает содержимое веб-страницы или API-эндпоинта по URL."""
    try:
        req = urllib.request.Request(
            url,
            headers={"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) ApiDuckAgent/2.0"}
        )
        with urllib.request.urlopen(req, timeout=15) as response:
            html = response.read().decode("utf-8", errors="replace")
            # Базовая очистка от скриптов/стилей для текстового чтения
            text = re.sub(r"<(script|style).*?</\1>", "", html, flags=re.DOTALL | re.IGNORECASE)
            text = re.sub(r"<[^>]+>", " ", text)
            text = re.sub(r"\s+", " ", text).strip()
            return text[:max_chars] if len(text) > max_chars else text
    except Exception as e:
        return f"Ошибка при загрузке URL {url}: {e}"

TOOLS = [
    {"type": "function", "function": {"name": "get_screen_size", "description": "Получить размер экрана.", "parameters": {"type": "object", "properties": {}}}},
    {"type": "function", "function": {"name": "screenshot", "description": "Сохранить снимок экрана в проект для анализа.", "parameters": {"type": "object", "properties": {"filepath": {"type": "string"}}}}},
    {"type": "function", "function": {"name": "mouse_move", "description": "Переместить мышь в координаты экрана.", "parameters": {"type": "object", "properties": {"x": {"type": "integer"}, "y": {"type": "integer"}, "duration": {"type": "number"}}, "required": ["x", "y"]}}},
    {"type": "function", "function": {"name": "mouse_click", "description": "Кликнуть мышью.", "parameters": {"type": "object", "properties": {"x": {"type": "integer"}, "y": {"type": "integer"}, "button": {"type": "string"}, "clicks": {"type": "integer"}}, "required": ["x", "y"]}}},
    {"type": "function", "function": {"name": "mouse_button", "description": "Нажать или отпустить кнопку мыши.", "parameters": {"type": "object", "properties": {"button": {"type": "string"}, "action": {"type": "string"}}}}},
    {"type": "function", "function": {"name": "mouse_scroll", "description": "Прокрутить колесо мыши.", "parameters": {"type": "object", "properties": {"amount": {"type": "integer"}, "x": {"type": "integer"}, "y": {"type": "integer"}}, "required": ["amount"]}}},
    {"type": "function", "function": {"name": "keyboard_type", "description": "Ввести текст с клавиатуры.", "parameters": {"type": "object", "properties": {"text": {"type": "string"}, "interval": {"type": "number"}}, "required": ["text"]}}},
    {"type": "function", "function": {"name": "keyboard_hotkey", "description": "Нажать комбинацию клавиш.", "parameters": {"type": "object", "properties": {"keys": {"type": "array", "items": {"type": "string"}}}, "required": ["keys"]}}},
    {
        "type": "function",
        "function": {
            "name": "list_files",
            "description": "Показывает список файлов и папок в директории проекта.",
            "parameters": {
                "type": "object",
                "properties": {
                    "directory": {"type": "string", "description": "Относительный путь к папке (по умолчанию '.')."}
                },
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "read_file",
            "description": "Читает содержимое файла из текущего проекта с возможностью чтения диапазона строк.",
            "parameters": {
                "type": "object",
                "properties": {
                    "filepath": {"type": "string", "description": "Относительный путь к файлу."},
                    "start_line": {"type": "integer", "description": "Начальная строка для чтения (начиная с 1, опционально)."},
                    "end_line": {"type": "integer", "description": "Конечная строка для чтения (опционально)."}
                },
                "required": ["filepath"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "write_file",
            "description": "Создает или полностью перезаписывает файл в текущем проекте.",
            "parameters": {
                "type": "object",
                "properties": {
                    "filepath": {"type": "string", "description": "Относительный путь к файлу."},
                    "content": {"type": "string", "description": "Полное содержимое файла."}
                },
                "required": ["filepath", "content"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "edit_file_part",
            "description": "Точечно заменяет кусок кода/текста (old_str) на новый (new_str) в файле без перезаписи всего файла.",
            "parameters": {
                "type": "object",
                "properties": {
                    "filepath": {"type": "string", "description": "Относительный путь к файлу."},
                    "old_str": {"type": "string", "description": "Точный исходный текст или фрагмент кода для замены."},
                    "new_str": {"type": "string", "description": "Новый текст или исправленный фрагмент кода."}
                },
                "required": ["filepath", "old_str", "new_str"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "append_to_file",
            "description": "Дописывает текст в конец файла (создаст файл, если он не существовал).",
            "parameters": {
                "type": "object",
                "properties": {
                    "filepath": {"type": "string", "description": "Относительный путь к файлу."},
                    "content": {"type": "string", "description": "Содержимое для добавления в конец."}
                },
                "required": ["filepath", "content"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "create_folder",
            "description": "Создает новую папку в проекте.",
            "parameters": {
                "type": "object",
                "properties": {
                    "folder_path": {"type": "string", "description": "Путь к создаваемой папке."}
                },
                "required": ["folder_path"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "delete_item",
            "description": "Удаляет файл или папку со всем содержимым из проекта.",
            "parameters": {
                "type": "object",
                "properties": {
                    "path": {"type": "string", "description": "Путь к файлу или папке."}
                },
                "required": ["path"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "move_item",
            "description": "Перемещает или переименовывает файл или папку в проекте.",
            "parameters": {
                "type": "object",
                "properties": {
                    "source_path": {"type": "string", "description": "Исходный путь."},
                    "destination_path": {"type": "string", "description": "Целевой путь."}
                },
                "required": ["source_path", "destination_path"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "get_project_tree",
            "description": "Возвращает полную структуру файлов и папок текущего проекта.",
            "parameters": {
                "type": "object",
                "properties": {
                    "directory": {"type": "string", "description": "Директория проекта (по умолчанию '.')."}
                },
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "search_code",
            "description": "Ищет фрагмент строки или регулярное выражение во всех файлах проекта (grep/ripgrep аналог).",
            "parameters": {
                "type": "object",
                "properties": {
                    "query": {"type": "string", "description": "Поисковый запрос или регулярное выражение."},
                    "file_pattern": {"type": "string", "description": "Фильтр файлов (например, '*.py', '*.js', по умолчанию '*.*')."},
                    "case_sensitive": {"type": "boolean", "description": "Учитывать регистр (по умолчанию false)."}
                },
                "required": ["query"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "find_by_name",
            "description": "Находит файлы и папки по названию или маске имени (glob-шаблон).",
            "parameters": {
                "type": "object",
                "properties": {
                    "pattern": {"type": "string", "description": "Шаблон имени (например, 'Dockerfile', '*.json', 'app.*')."}
                },
                "required": ["pattern"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "execute_command",
            "description": "Выполняет команду оболочки/терминала (bash, pip, git, npm, python тесты) в папке проекта.",
            "parameters": {
                "type": "object",
                "properties": {
                    "command": {"type": "string", "description": "Команда для исполнения в терминале."},
                    "timeout": {"type": "integer", "description": "Таймаут выполнения команды в секундах (по умолчанию 60)."}
                },
                "required": ["command"],
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "get_system_info",
            "description": "Возвращает информацию об операционной системе, версии Python и окружении.",
            "parameters": {
                "type": "object",
                "properties": {},
            },
        },
    },
    {
        "type": "function",
        "function": {
            "name": "fetch_web_page",
            "description": "Выполняет GET-запрос к веб-странице или API по указанному URL и возвращает текстовый контент.",
            "parameters": {
                "type": "object",
                "properties": {
                    "url": {"type": "string", "description": "URL адрес для загрузки (включая https://)."}
                },
                "required": ["url"],
            },
        },
    }
]

# ==========================================
# МЕНЕДЖЕР ПРОЕКТОВ
# ==========================================
def select_or_create_project(base_dir: str = "projects"):
    """Сканирует директорию проектов, позволяет выбрать существующий или создать новый."""
    os.makedirs(base_dir, exist_ok=True)

    while True:
        projects = [d for d in os.listdir(base_dir) if os.path.isdir(os.path.join(base_dir, d))]

        print("\n" + "="*50)
        print("📁 ДОСТУПНЫЕ ПРОЕКТЫ:")
        if projects:
            for idx, proj in enumerate(projects, 1):
                print(f"  [{idx}] {proj}")
        else:
            print("  (Пока нет ни одного проекта)")
        print("  [n] Создать новый проект")
        print("  [exit] Выход")
        print("="*50)

        choice = input("👉 Выберите номер проекта, 'n' или 'exit': ").strip().lower()

        if choice in ["exit", "выход", "quit"]:
            return None, None

        if choice == "n":
            new_name = input("✨ Введите имя нового проекта: ").strip()
            if new_name:
                project_path = os.path.join(base_dir, new_name)
                os.makedirs(project_path, exist_ok=True)
                print(f"✅ Проект '{new_name}' успешно создан.")
                return new_name, project_path
            else:
                print("❌ Имя проекта не может быть пустым.")

        elif choice.isdigit():
            idx = int(choice) - 1
            if 0 <= idx < len(projects):
                proj_name = projects[idx]
                project_path = os.path.join(base_dir, proj_name)
                print(f"📂 Выбран проект: '{proj_name}'")
                return proj_name, project_path
            else:
                print("❌ Неверный номер проекта.")
        else:
            print("❌ Неверный ввод. Попробуйте снова.")

# ==========================================
# КЛАСС АВТОНОМНОГО АГЕНТА
# ==========================================
class ApiDuckAgent:
    def __init__(self, project_path: str, model: str = "gpt-5.6-terra"):
        self.project_path = project_path
        self.model = model
        self.history = [
            {
                "role": "system",
                "content": (
                    f"Ты — автономный senior-разработчик и системный инженер. "
                    f"Ты работаешь внутри каталога проекта '{os.path.abspath(project_path)}'.\n"
                    "Тебе предоставлен полный набор инструментов:\n"
                    "- Чтение, запись, дописывание и точечная замена фрагментов файлов (edit_file_part);\n"
                    "- Поиск по всей кодовой базе (search_code) и поиск файлов по шаблону (find_by_name);\n"
                    "- Выполнение команд в терминале (execute_command) для сборки, установки пакетов (pip/npm) и запуска тестов;\n"
                    "- Просмотр информации об окружении (get_system_info) и загрузка веб-страниц/документации (fetch_web_page).\n"
                    "- Управление экраном: screenshot, mouse_move/click/button/scroll, keyboard_type/hotkey (только при явном включении пользователем).\n"
                    "Всегда проверяй результаты изменений и тестируй создаваемый код. Перед опасными действиями на рабочем столе запрашивай подтверждение."
                )
            }
        ]

    def set_model(self, model_id: str):
        self.model = model_id
        print(f"🔄 Модель переключена на: {self.model}")

    def execute_tool(self, fn_name: str, fn_args: dict) -> str:
        tools_map = {
            "get_screen_size": lambda **kw: get_screen_size(),
            "screenshot": lambda **kw: screenshot(self.project_path, **kw),
            "mouse_move": lambda **kw: mouse_move(**kw),
            "mouse_click": lambda **kw: mouse_click(**kw),
            "mouse_button": lambda **kw: mouse_button(**kw),
            "mouse_scroll": lambda **kw: mouse_scroll(**kw),
            "keyboard_type": lambda **kw: keyboard_type(**kw),
            "keyboard_hotkey": lambda **kw: keyboard_hotkey(**kw),
            "list_files": lambda **kw: list_files(self.project_path, **kw),
            "read_file": lambda **kw: read_file(self.project_path, **kw),
            "write_file": lambda **kw: write_file(self.project_path, **kw),
            "edit_file_part": lambda **kw: edit_file_part(self.project_path, **kw),
            "append_to_file": lambda **kw: append_to_file(self.project_path, **kw),
            "create_folder": lambda **kw: create_folder(self.project_path, **kw),
            "delete_item": lambda **kw: delete_item(self.project_path, **kw),
            "move_item": lambda **kw: move_item(self.project_path, **kw),
            "get_project_tree": lambda **kw: get_project_tree(self.project_path, **kw),
            "search_code": lambda **kw: search_code(self.project_path, **kw),
            "find_by_name": lambda **kw: find_by_name(self.project_path, **kw),
            "execute_command": lambda **kw: execute_command(self.project_path, **kw),
            "get_system_info": lambda **kw: get_system_info(self.project_path),
            "fetch_web_page": lambda **kw: fetch_web_page(**kw),
        }
        if fn_name in tools_map:
            try:
                return str(tools_map[fn_name](**fn_args))
            except Exception as e:
                return f"Ошибка при вызове {fn_name}: {e}"
        return f"Ошибка: запрошенный инструмент '{fn_name}' не найден."

    def chat(self, user_input: str) -> bool:
        """Обрабатывает ввод пользователя. Возвращает False, если пользователь запросил переключение проекта."""
        if user_input.lower() in ["/switch", "сменить", "назад"]:
            return False

        self.history.append({"role": "user", "content": user_input})

        while True:
            try:
                response = client.chat.completions.create(
                    model=self.model,
                    messages=self.history,
                    tools=TOOLS,
                    tool_choice="auto"
                )
            except Exception as e:
                print(f"\n❌ Ошибка обращения к API: {e}\n")
                break

            message = response.choices[0].message

            if message.tool_calls:
                self.history.append(message)
                for tool_call in message.tool_calls:
                    fn_name = tool_call.function.name
                    try:
                        fn_args = json.loads(tool_call.function.arguments)
                    except json.JSONDecodeError:
                        fn_args = {}

                    print(f"🛠️ [Инструмент]: {fn_name}({json.dumps(fn_args, ensure_ascii=False)})")
                    result = self.execute_tool(fn_name, fn_args)

                    # Выводим краткий анонс результата инструмента пользователю
                    preview = result if len(result) < 300 else result[:300] + "..."
                    print(f"   ↳ Результат: {preview}")

                    self.history.append({
                        "role": "tool",
                        "tool_call_id": tool_call.id,
                        "content": result
                    })
                    # Передаём снимок обратно модели как vision-сообщение, а не только путь к файлу.
                    if fn_name == "screenshot":
                        try:
                            shot = json.loads(result)
                            shot_path = resolve_path(self.project_path, shot["path"])
                            with open(shot_path, "rb") as image_file:
                                encoded = base64.b64encode(image_file.read()).decode("ascii")
                            self.history.append({
                                "role": "user",
                                "content": [
                                    {"type": "text", "text": "Снимок экрана получен. Проанализируй его при необходимости."},
                                    {"type": "image_url", "image_url": {"url": "data:image/png;base64," + encoded}}
                                ]
                            })
                        except Exception as image_error:
                            self.history.append({"role": "user", "content": f"Не удалось передать изображение модели: {image_error}"})
                continue

            if message.content:
                print(f"\n🤖 [{self.model}]:\n{message.content}\n")
                self.history.append({"role": "assistant", "content": message.content})
                break

        return True

# ==========================================
# ИНТЕРФЕЙС И МЕНЮ
# ==========================================
def print_models_info():
    print("\n" + "📖 " + "="*48)
    print("      СПРАВОЧНИК ПО МОДЕЛЯМ И ИХ НАЗНАЧЕНИЮ")
    print("="*50)
    for key, info in AVAILABLE_MODELS.items():
        print(f"[{key}] {info['name']} ({info['id']})")
        print(f"    💡 Назначение: {info['desc']}\n")
    print("="*50 + "\n")

def print_menu(proj_name: str, current_model: str):
    print("\n" + "="*50)
    print(f" 🚀 Проект: [{proj_name}]")
    print("="*50)
    print(f" Текущая модель: {current_model}")
    print("-" * 50)
    print(" Команды:")
    print("  /switch - сменить или создать другой проект")
    print("  /model  - сменить модель ИИ")
    print("  /info   - шпаргалка по моделям")
    print("  /clear  - очистить историю диалога")
    print("  /exit   - завершить работу")
    print("="*50 + "\n")

def select_model_dialog(current_model: str) -> str:
    print_models_info()
    choice = input("Введите номер модели для выбора: ").strip()
    if choice in AVAILABLE_MODELS:
        return AVAILABLE_MODELS[choice]["id"]
    else:
        print("⚠️ Неверный выбор. Модель оставлена без изменений.")
        return current_model

def main():
    current_model = "gpt-5.6-terra"

    while True:
        proj_name, proj_path = select_or_create_project("projects")
        if not proj_name:
            print("👋 До свидания!")
            break

        agent = ApiDuckAgent(project_path=proj_path, model=current_model)
        print_menu(proj_name, agent.model)

        while True:
            try:
                user_input = input(f"[{proj_name}] 👤 Вы: ").strip()

                if not user_input:
                    continue

                if user_input.lower() in ["/exit", "exit", "quit"]:
                    print("👋 Завершение работы.")
                    return

                if user_input.lower() == "/clear":
                    agent.history = [agent.history[0]]
                    print("🧹 История диалога текущего проекта очищена.")
                    continue

                if user_input.lower() == "/info":
                    print_models_info()
                    continue

                if user_input.lower() == "/model":
                    current_model = select_model_dialog(agent.model)
                    agent.set_model(current_model)
                    continue

                if user_input.lower() in ["/switch", "сменить", "назад"]:
                    print("🔄 Возврат к меню проектов...")
                    break

                active = agent.chat(user_input)
                if not active:
                    break

            except (KeyboardInterrupt, EOFError):
                print("\n👋 Завершение работы.")
                return

if __name__ == "__main__":
    main()