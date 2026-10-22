"""Durable telemetry and Windows process lock."""
import json
import os
from datetime import datetime, timezone
from pathlib import Path
from .journal import Journal
from .models import encode


def event(settings, kind, payload):
    journal = Journal(settings.journal_path)
    try:
        journal.db.execute('CREATE TABLE IF NOT EXISTS events (id INTEGER PRIMARY KEY, time TEXT, kind TEXT, payload TEXT)')
        stamp = datetime.now(timezone.utc).isoformat()
        data = json.dumps(encode(payload), ensure_ascii=False, allow_nan=False)
        with journal.db:
            journal.db.execute('INSERT INTO events(time,kind,payload) VALUES (?,?,?)', (stamp, kind, data))
        with Path('state/transactions.jsonl').open('a', encoding='utf-8') as stream:
            stream.write(json.dumps({'time': stamp, 'kind': kind, 'data': json.loads(data)}, ensure_ascii=False) + '\n')
    finally:
        journal.close()


class ProcessLock:
    def __init__(self, path='state/daemon.lock'):
        self.path = path

    def __enter__(self):
        import msvcrt
        Path('state').mkdir(exist_ok=True)
        self.stream = open(self.path, 'a+b')
        self.stream.seek(0, 2)
        if self.stream.tell() == 0:
            self.stream.write(b'0')
            self.stream.flush()
        self.stream.seek(0)
        try:
            msvcrt.locking(self.stream.fileno(), msvcrt.LK_NBLCK, 1)
        except OSError:
            self.stream.close()
            raise RuntimeError('Another bot process holds the trading lock')
        return self

    def __exit__(self, *args):
        import msvcrt
        self.stream.seek(0)
        msvcrt.locking(self.stream.fileno(), msvcrt.LK_UNLCK, 1)
        self.stream.close()
