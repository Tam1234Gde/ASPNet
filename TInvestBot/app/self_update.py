"""Безопасное управление изменениями кода: выключено по умолчанию."""
from __future__ import annotations
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]

def status() -> str:
    try:
        return subprocess.check_output(['git', 'status', '--short'], cwd=ROOT, text=True, stderr=subprocess.STDOUT).strip() or 'чисто'
    except Exception:
        return 'Git-репозиторий не инициализирован'

def snapshot_commit(message='learning checkpoint') -> str:
    if not (ROOT / '.git').exists():
        raise RuntimeError('Автоперезапись отключена: проект не является Git-репозиторием')
    subprocess.run(['git','add','app','README.md','reports'], cwd=ROOT, check=True)
    return subprocess.check_output(['git','commit','-m',message], cwd=ROOT, text=True, stderr=subprocess.STDOUT)

def rollback(ref='HEAD~1') -> str:
    if not (ROOT / '.git').exists():
        raise RuntimeError('Откат недоступен: Git-репозиторий не инициализирован')
    return subprocess.check_output(['git','reset','--hard',ref], cwd=ROOT, text=True, stderr=subprocess.STDOUT)
