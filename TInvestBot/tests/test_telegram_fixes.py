import asyncio
import time
from types import SimpleNamespace as NS
from app.control import Control
from app.telegram import Commands, _money, _portfolio_text, _balance_text, _instrument_line


def test_money_sdk_and_json():
    assert _money(NS(units=123, nano=500000000, currency='rub')) == '123.50 ₽'
    assert _money({'units': '123', 'nano': 500000000, 'currency': 'usd'}) == '123.50 USD'


def test_balance_and_lots():
    money = NS(units=100, nano=0, currency='rub')
    p = NS(total_amount_portfolio=money, total_amount_currencies=money, expected_yield=2,
           positions=[NS(figi='BBG123456789', quantity=20, quantity_lots=2,
                         average_position_price=money, expected_yield=-10)])
    text = _balance_text(p, NS(money=[money], blocked=[NS(units=30,nano=0,currency='rub')]))
    assert 'доступно 70.00' in text
    assert '100.00 ₽' in text
    text = _portfolio_text(p)
    assert '2 лот(ов), 20 шт.' in text
    assert 'убыток -10.00 ₽' in text


def test_figi_currency_only():
    assert _instrument_line({'figi':'ABC', 'currency':'rub','name':'secret'}) == 'ABC — рублёвый'
    assert _instrument_line({'figi':'XYZ', 'currency':''}) == 'XYZ — валюта не указана'


def test_friend_access_persistence_and_revocation(tmp_path):
    c = Control(tmp_path/'control.db')
    cmd = Commands(NS(), c, {1})
    async def send(user, text):
        return await cmd.handle({'from':{'id':user}, 'chat':{'type':'private','id':user}, 'date':time.time(),'text':text})
    async def scenario():
        assert 'Telegram ID: 2' in await send(2, '/id')
        assert await send(2, '/balance') is None
        assert 'Подтвердите' in await send(1, '/friend_add 2')
        assert 2 not in cmd.allowed_users()
        await send(1, '/confirm '+cmd.pending[1][0])
        assert 2 in Commands(NS(), c, {1}).allowed_users()
        assert 'владельцу' in await send(2, '/friend_add 3')
        assert 'владельцу' in await send(2, '/set interval 30')
        assert 'Пауза сохранена' in await send(2, '/pause')
        await send(2, '/resume')
        await send(1, '/friend_remove 2')
        assert 2 not in cmd.pending
        assert await send(2, '/pause') is None
    asyncio.run(scenario())
