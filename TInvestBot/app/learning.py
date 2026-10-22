"""Безопасное накопление опыта: факты и статистика, без саморедактирования кода."""
from __future__ import annotations
import json
from collections import Counter
from datetime import datetime, timezone
from pathlib import Path

class LearningStore:
    def __init__(self, path='state/learning.json'):
        self.path = Path(path); self.path.parent.mkdir(parents=True, exist_ok=True)
        self.data = self._load()
    def _load(self):
        try:
            data=json.loads(self.path.read_text(encoding='utf-8'))
            if isinstance(data,dict): return data
        except (OSError, ValueError): pass
        return {'version': 1, 'observations': 0, 'actions': Counter(), 'outcomes': Counter(), 'by_instrument': {}}
    def _save(self):
        tmp=self.path.with_suffix('.tmp')
        tmp.write_text(json.dumps(self.data, ensure_ascii=False, indent=2, default=dict), encoding='utf-8'); tmp.replace(self.path)
    def record(self, *, action, instrument='', outcome='unknown', confidence=None, pnl=None, source='daemon'):
        self.data['observations']=int(self.data.get('observations',0))+1
        for key,value in [('actions',action),('outcomes',outcome)]:
            bucket=self.data.setdefault(key,{}) ; bucket[value]=int(bucket.get(value,0))+1
        item=self.data.setdefault('by_instrument',{}).setdefault(instrument, {'observations':0,'outcomes':{}})
        item['observations']+=1; item['outcomes'][outcome]=int(item['outcomes'].get(outcome,0))+1
        self.data['last_update']=datetime.now(timezone.utc).isoformat()
        self._save()
    def summary(self):
        return {'version':self.data.get('version',1),'observations':self.data.get('observations',0),
                'actions':self.data.get('actions',{}),'outcomes':self.data.get('outcomes',{}),
                'instruments':len(self.data.get('by_instrument',{})),'last_update':self.data.get('last_update','нет')}
