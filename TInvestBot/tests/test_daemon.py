import asyncio
import json
from types import SimpleNamespace
from unittest.mock import AsyncMock
import pytest
from app.runtime import ProcessLock, event
from app.journal import Journal
from app.daemon import reconcile, stopped


def test_lock(tmp_path, monkeypatch):
    monkeypatch.chdir(tmp_path)
    with ProcessLock():
        with pytest.raises(RuntimeError):
            with ProcessLock():
                pass
    with ProcessLock():
        pass


def test_event_and_stop(tmp_path, monkeypatch):
    monkeypatch.chdir(tmp_path)
    (tmp_path / 'state').mkdir()
    s = SimpleNamespace(journal_path='state/test.sqlite3')
    event(s, 'TEST', {'lots': 1})
    assert json.loads((tmp_path / 'state/transactions.jsonl').read_text())['kind'] == 'TEST'
    assert not stopped()
    (tmp_path / 'state/stop.request').write_text('stop', encoding='utf-8-sig')
    assert stopped()


@pytest.mark.parametrize('pending,terminal,expected', [({'directive': {}}, False, True), ({'order_id': 'x'}, False, True), ({'order_id': 'x'}, True, False)])
def test_reconcile(tmp_path, monkeypatch, pending, terminal, expected):
    monkeypatch.chdir(tmp_path)
    (tmp_path / 'state').mkdir()
    s = SimpleNamespace(journal_path='state/test.sqlite3', account_id='sandbox', max_drawdown_rub=100)
    j = Journal(s.journal_path)
    j.put('pending', pending)
    j.close()
    money = SimpleNamespace(units=1000, nano=0)
    # Use dataclasses so production encoder is exercised.
    from dataclasses import make_dataclass
    Portfolio = make_dataclass('Portfolio', [('total_amount_portfolio', object)])
    Money = make_dataclass('Money', [('units', int), ('nano', int)])
    State = make_dataclass('State', [('order_id', str), ('execution_report_status', object), ('lots_executed', int), ('lots_requested', int), ('executed_commission', object)])
    from t_tech.invest import OrderExecutionReportStatus
    report = OrderExecutionReportStatus.EXECUTION_REPORT_STATUS_FILL if terminal else OrderExecutionReportStatus.EXECUTION_REPORT_STATUS_PARTIALLYFILL
    service = SimpleNamespace(get_sandbox_orders=AsyncMock(return_value=SimpleNamespace(orders=[])), get_sandbox_operations=AsyncMock(return_value=SimpleNamespace(operations=[])))
    broker = SimpleNamespace(services=SimpleNamespace(sandbox=service), portfolio=AsyncMock(return_value=Portfolio(Money(1000, 0))), order_state=AsyncMock(return_value=State('x', report, 1, 2, Money(0, 0))))
    assert asyncio.run(reconcile(s, broker)) == expected
