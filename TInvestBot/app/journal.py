"""Durable intent log: an uncertain submission blocks new orders across restarts."""
import json
import sqlite3
from pathlib import Path
from .models import encode

class Journal:
    def __init__(self, path):
        Path(path).parent.mkdir(parents=True, exist_ok=True)
        self.db = sqlite3.connect(path)
        self.db.execute('CREATE TABLE IF NOT EXISTS kv (key TEXT PRIMARY KEY, value TEXT NOT NULL)')
        self.db.commit()

    def get(self, key, default=None):
        row = self.db.execute('SELECT value FROM kv WHERE key=?', (key,)).fetchone()
        return json.loads(row[0]) if row else default

    def put(self, key, value):
        with self.db:
            self.db.execute('INSERT OR REPLACE INTO kv VALUES (?,?)',
                            (key, json.dumps(encode(value), allow_nan=False)))

    def close(self):
        self.db.close()
