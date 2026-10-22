"""Rebuild auditable FIFO statistics from deduplicated broker operations.
Only RUB long positions are supported. Unknown cost basis is never invented.
"""
import csv
import json
from collections import defaultdict, deque
from decimal import Decimal
from pathlib import Path


def money(value):
    return Decimal(str(value.get('units', 0))) + Decimal(str(value.get('nano', 0))) / Decimal(1000000000)


def calculate(operations):
    inventory = defaultdict(deque)
    rows = []
    gross = Decimal(0)
    fees = Decimal(0)
    cashflows = Decimal(0)
    issues = []
    closed = 0
    allocated_fees = defaultdict(lambda: Decimal(0))
    unallocated_fees = False
    net = Decimal(0)
    trade_ids = {item['id'] for item in operations if item.get('operation_type') in {'OPERATION_TYPE_BUY', 'OPERATION_TYPE_SELL'}}
    for item in operations:
        if item.get('state') == 'OPERATION_STATE_EXECUTED' and item.get('operation_type') == 'OPERATION_TYPE_BROKER_FEE':
            parent = item.get('parent_operation_id')
            if parent in trade_ids:
                allocated_fees[parent] += abs(money(item['payment']))
            else:
                unallocated_fees = True
    for item in sorted(operations, key=lambda entry: (entry['date'], entry['id'])):
        if item.get('state') != 'OPERATION_STATE_EXECUTED':
            continue
        kind = item.get('operation_type', '')
        payment = money(item['payment'])
        currency = item.get('currency', '').lower()
        instrument = item.get('instrument_uid') or item.get('figi', '')
        row = {'id': item['id'], 'date': item['date'], 'type': kind,
               'instrument': instrument, 'currency': currency, 'quantity': item.get('quantity', 0),
               'payment': str(payment), 'fifo_gross_pnl_rub': ''}
        rows.append(row)
        if currency != 'rub':
            issues.append('Non-RUB operation: ' + item['id'])
            continue
        if kind in {'OPERATION_TYPE_INPUT', 'OPERATION_TYPE_OUTPUT'}:
            cashflows += payment
        elif kind == 'OPERATION_TYPE_BROKER_FEE':
            fees += abs(payment)
        elif kind in {'OPERATION_TYPE_BUY', 'OPERATION_TYPE_SELL'}:
            quantity = Decimal(item.get('quantity', 0)) - Decimal(item.get('quantity_rest', 0))
            if quantity <= 0:
                issues.append('Missing executed quantity: ' + item['id'])
                continue
            if kind == 'OPERATION_TYPE_BUY':
                inventory[instrument].append([quantity, abs(payment) / quantity, allocated_fees[item['id']] / quantity])
            else:
                remaining = quantity
                cost = Decimal(0)
                buy_fees = Decimal(0)
                while remaining and inventory[instrument]:
                    lot = inventory[instrument][0]
                    used = min(remaining, lot[0])
                    cost += used * lot[1]
                    buy_fees += used * lot[2]
                    remaining -= used
                    lot[0] -= used
                    if lot[0] == 0:
                        inventory[instrument].popleft()
                if remaining:
                    issues.append('Unknown opening cost / short position: ' + item['id'])
                else:
                    profit = payment - cost
                    row['fifo_gross_pnl_rub'] = str(profit)
                    gross += profit
                    net += profit - buy_fees - allocated_fees[item['id']]
                    closed += 1
        elif kind not in {'OPERATION_TYPE_INPUT', 'OPERATION_TYPE_OUTPUT'}:
            issues.append('Unsupported accounting operation: ' + item['id'])
    return {'closed_sell_operations': closed, 'fifo_gross_pnl_rub': str(gross) if not issues else None,
            'broker_fees_rub': str(fees), 'net_cashflows_rub': str(cashflows),
            'realized_pnl_rub': str(net) if not issues and not unallocated_fees else None,
            'accounting_notes': ['Provisional FIFO for downloaded RUB history; late fees cause recalculation. Unlinked fees or unknown cost basis make net PnL unavailable. Not a broker tax report.'] + issues,
            'unallocated_fees': unallocated_fees,
            'open_inventory_units': {key: str(sum((lot[0] for lot in lots), Decimal(0))) for key, lots in inventory.items()}}, rows


def export(journal):
    operations = [json.loads(row[0]) for row in journal.db.execute('SELECT payload FROM operations')]
    summary, rows = calculate(operations)
    temporary = Path('state/operations.csv.tmp')
    with temporary.open('w', encoding='utf-8-sig', newline='') as stream:
        writer = csv.DictWriter(stream, fieldnames=['id', 'date', 'type', 'instrument', 'currency', 'quantity', 'payment', 'fifo_gross_pnl_rub'], delimiter=';')
        writer.writeheader()
        writer.writerows(rows)
    temporary.replace('state/operations.csv')
    return summary
