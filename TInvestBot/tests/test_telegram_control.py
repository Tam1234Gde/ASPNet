import asyncio
import time
from dataclasses import replace
from unittest.mock import AsyncMock
from app.config import Settings
from app.control import Control
from app.telegram import Commands
from app.multi import multi_cycle


def message(text, user=7, kind='private'):
    return {'from':{'id':user}, 'chat':{'id':user,'type':kind}, 'date':int(time.time()),'text':text}


def test_access_and_stale(tmp_path):
    c = Control(tmp_path/'c.db')
    cmd = Commands(Settings(),c,{7})
    assert asyncio.run(cmd.handle(message('/resume',8))) is None
    assert asyncio.run(cmd.handle(message('/resume',7,'group'))) is None
    m = message('/pause'); m['date'] = 1
    assert 'устарела' in asyncio.run(cmd.handle(m))
    assert c.get('paused') is None


def test_confirmation_and_persistence(tmp_path):
    c = Control(tmp_path/'c.db'); cmd = Commands(Settings(),c,{7})
    asyncio.run(cmd.handle(message('/resume')))
    assert c.get('paused',True)
    code = cmd.pending[7][0]
    asyncio.run(cmd.handle(message('/confirm '+code)))
    assert not Control(tmp_path/'c.db').get('paused')
    assert 'Неверное' in asyncio.run(cmd.handle(message('/confirm '+code)))
    asyncio.run(cmd.handle(message('/pause')))
    assert c.get('paused')


def test_modes_limits_instruments(tmp_path):
    s = replace(Settings(),instrument_uids=('a','b'),dry_run=False)
    c = Control(tmp_path/'c.db')
    assert c.effective(s).dry_run
    c.put('paused',False); c.put('mode','sandbox'); c.put('instruments',{'a':False,'b':True})
    assert c.effective(s).instrument_uids == ('b',)
    assert not c.permitted(s,'a') and c.permitted(s,'b')
    assert c.effective(replace(s,dry_run=True)).dry_run
    for name,value in [('token','123'),('interval','0'),('max_order_lots','NaN')]:
        try:
            c.set_limit(name,value)
            assert False
        except ValueError:
            pass
    c.set_limit('interval','90')
    assert c.get('interval') == 90


def test_multi_isolation_and_reconciliation(tmp_path,monkeypatch):
    monkeypatch.chdir(tmp_path)
    c = Control(); c.put('paused',False)
    s = replace(Settings(),instrument_uids=('a','b'))
    fn = AsyncMock(side_effect=[ValueError('invalid'),None])
    reconcile = AsyncMock(return_value=False)
    monkeypatch.setattr('app.multi.event',lambda *a:None)
    asyncio.run(multi_cycle(s,None,None,fn,reconcile))
    assert [x.args[0].instrument_uids for x in fn.await_args_list] == [('a',),('b',)]
    assert reconcile.await_count == 4


def test_multi_stops_on_pending(tmp_path,monkeypatch):
    monkeypatch.chdir(tmp_path)
    Control().put('paused',False)
    fn = AsyncMock()
    reconcile = AsyncMock(side_effect=[False,True])
    asyncio.run(multi_cycle(replace(Settings(),instrument_uids=('a','b')),None,None,fn,reconcile))
    assert fn.await_count == 1


def test_expired_confirmation(tmp_path):
    cmd = Commands(Settings(),Control(tmp_path/'c.db'),{7})
    cmd.pending[7] = ('code',0,'/resume',[])
    assert 'просроченное' in asyncio.run(cmd.handle(message('/confirm code')))


def test_telegram_transport_mock():
    import httpx
    from app.telegram import TelegramAPI
    async def check():
        api = TelegramAPI('dummy')
        await api.client.aclose()
        calls=[]
        def transport(request):
            calls.append(request)
            return httpx.Response(200,json={'ok':True,'result':True})
        api.client=httpx.AsyncClient(transport=httpx.MockTransport(transport))
        await api.send(7,'x'*6001)
        assert len(calls)==3
        await api.client.aclose()
    asyncio.run(check())
