"""Persistent control plane. No secrets, no arbitrary environment edits."""
import json
import sqlite3
from dataclasses import replace
from pathlib import Path

class Control:
    def __init__(self, path='state/control.sqlite3'):
        self.path = str(path)
        Path(path).parent.mkdir(parents=True, exist_ok=True)
        with self.connect() as db:
            db.execute('CREATE TABLE IF NOT EXISTS kv (key TEXT PRIMARY KEY, value TEXT NOT NULL)')

    def connect(self):
        return sqlite3.connect(self.path, timeout=10)

    def get(self, key, default=None):
        with self.connect() as db:
            row = db.execute('SELECT value FROM kv WHERE key=?', (key,)).fetchone()
        return json.loads(row[0]) if row else default

    def put(self, key, value):
        with self.connect() as db:
            db.execute('INSERT OR REPLACE INTO kv VALUES (?,?)', (key, json.dumps(value)))

    def instruments(self, s):
        return self.get('instruments', {uid: True for uid in s.instrument_uids})

    def effective(self, s):
        return replace(s, instrument_uids=tuple(k for k,v in self.instruments(s).items() if v),
                       dry_run=s.dry_run or self.get('mode', 'observe') != 'sandbox',
                       max_order_lots=max(1, int(self.get('max_order_lots', s.max_order_lots))),
                       max_drawdown_rub=self.get('max_drawdown_rub', s.max_drawdown_rub))

    def permission_reasons(self, s, uid):
        """Return explicit local reasons why a new order is blocked."""
        reasons = []
        if self.get('paused', True):
            reasons.append('бот находится на паузе')
        mode = self.get('mode', 'observe')
        if mode != 'sandbox':
            reasons.append(f'режим «{mode}»: торговля разрешена только в sandbox')
        instruments = self.instruments(s)
        if uid not in instruments:
            reasons.append('инструмент отсутствует в локальном списке')
        elif not instruments.get(uid, False):
            reasons.append('инструмент выключен в локальном списке')
        return reasons

    def permitted(self, s, uid):
        return not self.permission_reasons(s, uid)

    def set_limit(self, name, value):
        bounds = {'interval': (30,3600), 'notify_interval': (60,86400), 'max_order_lots': (1, 10_000_000),
                  'max_drawdown_rub': (1,1000000), 'max_exposure_rub': (1,10000000)}
        if name not in bounds:
            raise ValueError('Unknown setting')
        value = int(value)
        low, high = bounds[name]
        if not low <= value <= high:
            raise ValueError('Out of range')
        self.put(name, value)
