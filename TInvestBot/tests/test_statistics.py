from app.statistics import calculate


def operation(identity, kind, payment, quantity=0, parent=''):
    return {'id': identity, 'date': identity, 'operation_type': 'OPERATION_TYPE_' + kind,
            'state': 'OPERATION_STATE_EXECUTED', 'payment': {'units': payment, 'nano': 0},
            'currency': 'rub', 'instrument_uid': 'share', 'quantity': quantity,
            'quantity_rest': 0, 'parent_operation_id': parent}


def test_partial_fifo_and_linked_fees():
    result, rows = calculate([operation('1', 'BUY', -1000, 10), operation('2', 'BROKER_FEE', -10, parent='1'),
                              operation('3', 'SELL', 600, 5), operation('4', 'BROKER_FEE', -6, parent='3')])
    assert result['fifo_gross_pnl_rub'] == '100'
    assert result['realized_pnl_rub'] == '89'
    assert result['open_inventory_units']['share'] == '5'
    assert len(rows) == 4


def test_unknown_cost_not_profit():
    result, _ = calculate([operation('1', 'SELL', 600, 5)])
    assert result['realized_pnl_rub'] is None


def test_unlinked_commission_not_profit():
    result, _ = calculate([operation('1', 'BROKER_FEE', -10)])
    assert result['realized_pnl_rub'] is None


def test_deposit_not_profit():
    result, _ = calculate([operation('1', 'INPUT', 200000)])
    assert result['realized_pnl_rub'] == '0'
    assert result['net_cashflows_rub'] == '200000'
