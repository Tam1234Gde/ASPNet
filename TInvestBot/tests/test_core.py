from decimal import Decimal
import pytest
from app.models import decimal_to_quotation, quotation_to_decimal, Directive
from app.ai import AIClient
from app.risk import CircuitBreaker

def test_quotation_roundtrip():
    u, n = decimal_to_quotation(Decimal("123.456789012")); assert (u, n) == (123, 456789012)

def test_directive():
    d = Directive.from_json({"ACTION":"BUY", "INSTRUMENT_ID":"x", "LOTS":1, "CONFIDENCE":.9})
    assert d.instrument_id == "x" and d.lots == 1

def test_breaker():
    b = CircuitBreaker(Decimal(100), Decimal(1000)); assert not b.update(950); assert b.update(899)

def test_ai_response_parsing():
    assert AIClient._json_from_response({"content": [{"type": "text", "text": '{"ACTION":"HOLD","INSTRUMENT_ID":"x","LOTS":0,"CONFIDENCE":0}'}]})["ACTION"] == "HOLD"


def test_control_permission_reasons_are_specific(tmp_path):
    from app.control import Control
    from types import SimpleNamespace
    c = Control(tmp_path / 'control.sqlite3')
    c.put('paused', True)
    c.put('mode', 'observe')
    c.put('instruments', {'uid-1': False})
    reasons = c.permission_reasons(SimpleNamespace(instrument_uids=('uid-1',)), 'uid-1')
    assert 'бот находится на паузе' in reasons
    assert any('sandbox' in reason for reason in reasons)
    assert 'инструмент выключен в локальном списке' in reasons
