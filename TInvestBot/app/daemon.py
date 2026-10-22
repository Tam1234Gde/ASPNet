"""Sandbox-only continuous worker. Never retries uncertain submissions."""
import argparse
import asyncio
import json
import logging
import os
from dataclasses import replace
from datetime import datetime, timedelta, timezone
from decimal import Decimal
from pathlib import Path
from .config import Settings
from .ai import AIClient
from .tbank import TBankClient
from .main import cycle
from .journal import Journal
from .models import encode, quotation_to_decimal as dec
from .runtime import ProcessLock, event

TERMINAL = {'EXECUTION_REPORT_STATUS_FILL', 'EXECUTION_REPORT_STATUS_REJECTED', 'EXECUTION_REPORT_STATUS_CANCELLED'}


def stopped():
    path = Path('state/stop.request')
    return path.exists() and path.read_text(encoding='utf-8-sig').strip() == 'stop'


def status(phase, **details):
    data = {'pid': os.getpid(), 'time': datetime.now(timezone.utc).isoformat(), 'phase': phase, **details}
    Path('state/status.json').write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding='utf-8')


async def reconcile(s, broker):
    journal = Journal(s.journal_path)
    try:
        owner = journal.get('account_scope')
        if owner is not None and owner != s.account_id:
            raise RuntimeError('Journal belongs to another account; use a separate project/state folder')
        journal.put('account_scope', s.account_id)
        pending = journal.get('pending')
        blocked = False
        if pending:
            if not pending.get('order_id'):
                logging.error('UNCERTAIN_SUBMISSION: inspect broker operations; automatic orders blocked')
                blocked = True
            else:
                state = await broker.order_state(pending['order_id'])
                event(s, 'ORDER_RECONCILED', state)
                logging.info('ORDER_STATE id=%s status=%s filled=%s requested=%s commission=%s', state.order_id, state.execution_report_status.name, state.lots_executed, state.lots_requested, dec(state.executed_commission))
                if state.execution_report_status.name in TERMINAL:
                    journal.put('pending', None)
                else:
                    blocked = True
        active = await broker.services.sandbox.get_sandbox_orders(account_id=s.account_id)
        if active.orders:
            event(s, 'ACTIVE_ORDERS', active.orders)
            blocked = True
        # Overlapping reads and durable deduplication handle late broker updates.
        now = datetime.now(timezone.utc)
        since = journal.get('operations_since')
        start = datetime.fromisoformat(since) - timedelta(days=1) if since else now - timedelta(days=30)
        response = await broker.services.sandbox.get_sandbox_operations(account_id=s.account_id, from_=start, to=now)
        journal.db.execute('CREATE TABLE IF NOT EXISTS operations (id TEXT PRIMARY KEY, payload TEXT NOT NULL)')
        for operation in response.operations:
            data = json.dumps(encode(operation), sort_keys=True, ensure_ascii=False)
            previous = journal.db.execute('SELECT payload FROM operations WHERE id=?', (operation.id,)).fetchone()
            if not previous or previous[0] != data:
                event(s, 'BROKER_OPERATION', operation)
                with journal.db:
                    journal.db.execute('INSERT OR REPLACE INTO operations VALUES (?,?)', (operation.id, data))
        journal.put('operations_since', now.isoformat())
        from .statistics import export
        accounting = export(journal)
        portfolio = await broker.portfolio()
        event(s, 'ACCOUNT_SNAPSHOT', portfolio)
        equity = dec(portfolio.total_amount_portfolio)
        baseline = journal.get('daemon_baseline_equity')
        if baseline is None:
            journal.put('daemon_baseline_equity', str(equity))
            baseline = str(equity)
        flow_baseline = journal.get('daemon_baseline_cashflows')
        if flow_baseline is None:
            flow_baseline = accounting['net_cashflows_rub']
            journal.put('daemon_baseline_cashflows', flow_baseline)
        adjusted_change = equity - Decimal(baseline) - (Decimal(accounting['net_cashflows_rub']) - Decimal(flow_baseline))
        drawdown = -adjusted_change
        if drawdown >= Decimal(str(s.max_drawdown_rub)):
            journal.put('daemon_halted', True)
        if journal.get('daemon_halted'):
            logging.error('DRAWDOWN_HALTED: new orders blocked; positions are NOT automatically closed')
            blocked = True
        summary = {'equity_rub': str(equity), 'baseline_equity_rub': baseline,
                   'equity_change_rub': str(equity - Decimal(baseline)),
                   'realized_pnl_rub': None, 'note': 'Equity change is not realized profit; deposits/withdrawals affect it.',
                   'active_orders': len(active.orders), 'blocked': blocked,
                   'operations_in_database': journal.db.execute('SELECT COUNT(*) FROM operations').fetchone()[0]}
        summary.update(accounting)
        summary['cashflow_adjusted_equity_change_rub'] = str(adjusted_change)
        summary['updated_at'] = now.isoformat()
        temporary = Path('state/statistics.json.tmp')
        temporary.write_text(json.dumps(summary, ensure_ascii=False, indent=2), encoding='utf-8')
        temporary.replace('state/statistics.json')
        return blocked
    finally:
        journal.close()


async def iteration(s):
    async with TBankClient(s) as broker:
        blocked = await reconcile(s, broker)
        if stopped():
            return
        if blocked:
            logging.warning('CYCLE_BLOCKED: reconciliation/risk lock; monitoring continues')
            return
        ai = AIClient(s)
        try:
            from .multi import multi_cycle
            await multi_cycle(s, ai, broker, cycle, reconcile)
        finally:
            await ai.close()
        await reconcile(s, broker)


async def run(s, interval, once):
    failures = 0
    number = 0
    event(s, 'DAEMON_START', {'sandbox': s.sandbox, 'dry_run': s.dry_run, 'interval': interval})
    while not stopped():
        number += 1
        status('cycle', cycle=number, dry_run=s.dry_run)
        try:
            from .control import Control
            control = Control()
            effective = control.effective(s)
            await asyncio.wait_for(iteration(effective), timeout=(s.ai_timeout + 120) * max(1, len(effective.instrument_uids)))
            failures = 0
            logging.info('DAEMON_CYCLE_COMPLETE number=%s', number)
            status('waiting', cycle=number, dry_run=effective.dry_run,
                   paused=control.get('paused', True), instruments=list(effective.instrument_uids))
        except Exception as exc:
            failures += 1
            # Do not log exception text: API responses can contain sensitive data.
            logging.error('DAEMON_CYCLE_FAILED number=%s type=%s failures=%s', number, type(exc).__name__, failures)
            event(s, 'ERROR', {'cycle': number, 'type': type(exc).__name__})
            status('error_backoff', cycle=number, error=type(exc).__name__, dry_run=s.dry_run)
        if once:
            break
        from .control import Control
        delay = min(3600, Control().get('interval', interval) * (2 ** min(failures, 4)))
        for tick in range(int(delay)):
            if stopped():
                break
            await asyncio.sleep(1)
    event(s, 'DAEMON_STOP', {'cycles': number})
    status('stopped', cycles=number)
    logging.info('DAEMON_STOPPED: existing orders and positions remain at broker')


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--sandbox-trade', action='store_true', help='Explicit sandbox trading permission; ignores DRY_RUN only for this process')
    parser.add_argument('--once', action='store_true')
    parser.add_argument('--resume', action='store_true', help='Clear stop request only after acquiring process lock')
    parser.add_argument('--interval', type=int, default=60)
    args = parser.parse_args()
    s = Settings()
    if not s.sandbox:
        raise SystemExit('Production is blocked')
    s = replace(s, dry_run=not args.sandbox_trade)
    s.validate()
    if args.interval < 30 or not s.token or not s.account_id or not s.instrument_uids:
        raise SystemExit('Require credentials, instruments and interval >= 30 seconds')
    with ProcessLock():
        if args.resume:
            Path('state/stop.request').write_text('run', encoding='utf-8')
        Path('state/daemon.pid').write_text(str(os.getpid()), encoding='ascii')
        try:
            asyncio.run(run(s, args.interval, args.once))
        except KeyboardInterrupt:
            status('stopped_keyboard')


if __name__ == '__main__':
    main()
