import asyncio
from dataclasses import replace
from decimal import Decimal
from types import SimpleNamespace
from unittest.mock import AsyncMock
import pytest
from t_tech.invest import InstrumentIdType, Quotation
from app.config import Settings
from app.tbank import TBankClient
from app.models import encode, quotation_to_decimal
from app.journal import Journal


def test_installed_sdk_serialization():
    q = Quotation(units=12, nano=500000000)
    assert quotation_to_decimal(q) == Decimal('12.5')
    assert encode(q) == {'units': 12, 'nano': 500000000}

@pytest.mark.parametrize('identifier,kind', [
    ('BBG004730N88', InstrumentIdType.INSTRUMENT_ID_TYPE_FIGI),
    ('e6123145-9665-43e0-8413-cd61b8aa9b13', InstrumentIdType.INSTRUMENT_ID_TYPE_UID)])
def test_identifier_resolution(identifier, kind):
    broker = object.__new__(TBankClient)
    method = AsyncMock(return_value=SimpleNamespace(instrument='resolved'))
    broker.services = SimpleNamespace(instruments=SimpleNamespace(get_instrument_by=method))
    assert asyncio.run(broker.instrument(identifier)) == 'resolved'
    method.assert_awaited_once_with(id_type=kind, id=identifier)

@pytest.mark.parametrize('dry,sandbox', [(True, True), (True, False), (False, False)])
def test_adapter_blocks_orders(dry, sandbox):
    broker = object.__new__(TBankClient)
    broker.s = replace(Settings(), dry_run=dry, sandbox=sandbox)
    with pytest.raises(RuntimeError):
        asyncio.run(broker.post_order('uid', 1, True))


def test_pending_survives_restart(tmp_path):
    path = str(tmp_path / 'journal.sqlite3')
    journal = Journal(path)
    journal.put('pending', {'order_id': 'test'})
    journal.close()
    journal = Journal(path)
    assert journal.get('pending') == {'order_id': 'test'}
    journal.close()
