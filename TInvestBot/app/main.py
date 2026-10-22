import asyncio
import logging
from datetime import datetime, timezone
from decimal import Decimal
from time import monotonic
from .config import Settings
from .ai import AIClient
from .models import MarketSnapshot, Action, encode, quotation_to_decimal as dec
from .risk import CircuitBreaker
from .tbank import TBankClient

from .runtime import event, ProcessLock
from .observability import setup_logging
setup_logging()

async def cycle(s, ai, broker):
    from .control import Control
    control = Control()
    if control.get('paused', True):
        logging.info('PAUSED: monitoring only; no new order')
        return
    logging.info("CYCLE_START sandbox=%s dry_run=%s", s.sandbox, s.dry_run)
    portfolio = await broker.portfolio()
    event(s, "PORTFOLIO", portfolio)
    instruments, candles, books = [], {}, {}
    metadata = {}
    # Настройки могут хранить FIGI, а API/AI возвращают canonical UID. Сохраняем
    # соответствие, чтобы защита проверяла тот же включённый ключ, а не считала
    # разрешённый инструмент выключенным из-за разных идентификаторов.
    control_key_by_uid = {}
    for uid in s.instrument_uids:
        configured_id = uid
        item = await broker.instrument(uid)
        uid = item.uid
        control_key_by_uid[uid] = configured_id
        if item.instrument_type != 'share' or item.currency.lower() != 'rub':
            raise ValueError('Only RUB shares supported')
        metadata[uid] = item
        prices = await broker.services.market_data.get_last_prices(instrument_id=[uid])
        instruments.append({"instrument_id": uid, "lot": item.lot,
                            "min_price_increment": str(dec(item.min_price_increment)),
                            "last_prices": encode(prices.last_prices)})
        candles[uid] = encode((await broker.candles(uid)).candles)
        books[uid] = encode(await broker.services.market_data.get_order_book(instrument_id=uid, depth=s.orderbook_depth))
    snapshot = MarketSnapshot(datetime.now(timezone.utc).isoformat(), instruments,
                              orderbooks=books, candles=candles,
                              positions=encode(portfolio.positions), portfolio=encode(portfolio))
    logging.info("SNAPSHOT_READY instruments=%s equity=%s", len(instruments), dec(portfolio.total_amount_portfolio))
    d = await ai.decide(snapshot)
    logging.info("DECISION action=%s instrument=%s lots=%s limit=%s confidence=%s", d.action, d.instrument_id, d.lots, d.price_limit, d.confidence)
    event(s, "DECISION", d)
    if d.action == Action.HOLD:
        logging.info("HOLD: no order")
        return
    if d.instrument_id not in metadata:
        raise ValueError("AI returned instrument outside allowlist")
    if d.action == Action.HOLD:
        logging.info("HOLD: no order")
        return
    if s.dry_run:
        logging.info("DRY_RUN_DECISION: no order submitted; no simulated profit calculated")
        return
    # AI can take minutes: refresh execution inputs, never relax freshness limits.
    portfolio = await broker.portfolio()
    fresh_book = await broker.order_book(d.instrument_id, s.orderbook_depth)
    books[d.instrument_id] = encode(fresh_book)
    if d.price_limit is not None:
        levels = fresh_book.asks if d.action == Action.BUY else fresh_book.bids
        if not levels:
            raise ValueError('Fresh orderbook has no executable side')
        reference = dec(levels[0].price)
        if reference <= 0 or abs(d.price_limit - reference) / reference > Decimal('0.01'):
            raise ValueError('AI limit deviates more than 1 percent from fresh market')
    from pathlib import Path
    stop_path = Path('state/stop.request')
    if stop_path.exists() and stop_path.read_text(encoding='utf-8-sig').strip() == 'stop':
        logging.info('STOP_REQUESTED: no new order')
        return
    book = books[d.instrument_id]
    book_time = book.get('orderbook_ts') or book.get('time')
    if not book_time or (datetime.now(timezone.utc) - datetime.fromisoformat(book_time)).total_seconds() > s.max_snapshot_age_seconds:
        raise ValueError('Orderbook missing timestamp or stale')
    item = metadata[d.instrument_id]
    position = next((p for p in portfolio.positions if p.instrument_uid == d.instrument_id), None)
    lots = dec(position.quantity_lots) if position else Decimal(0)
    if d.action == Action.BUY and lots + d.lots > s.max_order_lots:
        raise ValueError('Total position ceiling exceeded')
    CircuitBreaker(Decimal(str(s.max_drawdown_rub)), Decimal(0)).validate(
        d, lot_size=item.lot, price_increment=dec(item.min_price_increment),
        position_lots=lots, max_lots=s.max_order_lots, allow_market=s.allow_market_orders)
    from t_tech.invest import SecurityTradingStatus
    status = await broker.services.market_data.get_trading_status(instrument_id=d.instrument_id)
    if status.trading_status != SecurityTradingStatus.SECURITY_TRADING_STATUS_NORMAL_TRADING:
        raise ValueError("Trading session is not normal trading")
    if not status.api_trade_available_flag:
        raise ValueError("API trading unavailable")
    if d.price_limit is None and not status.market_order_available_flag:
        raise ValueError("Market orders unavailable")
    if d.price_limit is not None and not status.limit_order_available_flag:
        raise ValueError("Limit orders unavailable")
    # Recheck remote controls after slow AI/network calls, immediately before intent.
    control_key = control_key_by_uid.get(d.instrument_id, d.instrument_id)
    permission_reasons = control.permission_reasons(s, control_key)
    if permission_reasons:
        reason = '; '.join(permission_reasons)
        event(s, 'RISK_REJECTED', {'instrument': d.instrument_id, 'reason': reason,
                                   'reasons': permission_reasons})
        logging.warning('ORDER_BLOCKED_LOCAL instrument=%s reasons=%s', d.instrument_id, reason)
        return
    current = control.effective(s)
    if current.max_order_lots != s.max_order_lots or current.max_drawdown_rub != s.max_drawdown_rub:
        raise ValueError('Risk settings changed during decision')
    if d.action == Action.BUY:
        positions = await broker.services.sandbox.get_sandbox_positions(account_id=s.account_id)
        cash = sum((dec(m) for m in positions.money if m.currency.lower() == 'rub'), Decimal(0))
        blocked_cash = sum((dec(m) for m in positions.blocked if m.currency.lower() == 'rub'), Decimal(0))
        cost = (d.price_limit or dec(fresh_book.asks[0].price)) * item.lot * d.lots
        exposure = dec(portfolio.total_amount_shares) + dec(portfolio.total_amount_bonds) + dec(portfolio.total_amount_etf)
        if cost * Decimal('1.01') > cash - blocked_cash or exposure + cost > Decimal(control.get('max_exposure_rub', 10000)):
            event(s, 'RISK_REJECTED', {'instrument':d.instrument_id, 'reason':'cash or total portfolio exposure limit'})
            return
    active = await broker.services.sandbox.get_sandbox_orders(account_id=s.account_id)
    if active.orders:
        raise ValueError('Active account orders block new submissions')
    from .journal import Journal
    journal = Journal(s.journal_path)
    try:
        if journal.get("pending"):
            raise RuntimeError("Unresolved previous order: manual reconciliation required")
        journal.put("pending", {"directive": encode(d), "time": datetime.now(timezone.utc).isoformat()})
    finally:
        journal.close()
    event(s, "ORDER_INTENT", d)
    start = monotonic()
    result = await broker.post_order(d.instrument_id, d.lots,
                                    d.action == Action.BUY or (d.action == Action.CLOSE and lots < 0), d.price_limit)
    logging.info("Order submitted: %s", result.order_id)
    event(s, "ORDER_SUBMITTED", result)
    journal = Journal(s.journal_path)
    try:
        journal.put("pending", {"order_id": result.order_id, "directive": encode(d)})
    finally:
        journal.close()
    # Никогда не переотправляем PostOrder при неизвестном результате исполнения.
    state = await broker.order_state(result.order_id)
    logging.info("ORDER_STATE id=%s status=%s filled=%s requested=%s commission=%s", result.order_id, state.execution_report_status.name, state.lots_executed, state.lots_requested, dec(state.executed_commission))
    event(s, "ORDER_STATE", state)
    journal = Journal(s.journal_path)
    try:
        journal.put("last_order", encode(state))
        if state.execution_report_status.name in {"EXECUTION_REPORT_STATUS_FILL", "EXECUTION_REPORT_STATUS_REJECTED", "EXECUTION_REPORT_STATUS_CANCELLED"}:
            journal.put("pending", None)
    finally:
        journal.close()
    await ai.feedback({"order_id": result.order_id, "directive": encode(d),
                       "state": encode(state), "realized_pnl_rub": None,
                       "execution_delay_ms": int((monotonic()-start)*1000),
                       "delay_kind": "submission_and_state_query", "market_context": encode(snapshot)})
    logging.info("Execution telemetry captured; filled lots: %s", state.lots_executed)

async def run_once(s: Settings, online=False):
    if s.dry_run and not online:
        logging.info("DRY_RUN: network and orders disabled; use python -m app.ai_check to check AI")
        return
    if not s.sandbox:
        raise RuntimeError("Production blocked: continuous circuit breaker/flatten and reconciliation are not implemented")
    if not s.instrument_uids:
        raise ValueError("INSTRUMENT_UIDS required")
    ai = AIClient(s)
    try:
        async with TBankClient(s) as broker:
            await cycle(s, ai, broker)
    finally:
        await ai.close()

def main():
    import argparse
    from dataclasses import replace
    parser = argparse.ArgumentParser()
    parser.add_argument("--observe", action="store_true", help="One online AI cycle, orders forcibly disabled")
    args = parser.parse_args()
    s = Settings()
    if args.observe:
        s = replace(s, dry_run=True)
    s.validate()
    try:
        with ProcessLock():
            asyncio.run(asyncio.wait_for(run_once(s, online=args.observe), timeout=max(90, s.ai_timeout + 60)))
        logging.info("RUN_COMPLETE: one cycle finished; process exits")
    except Exception as exc:
        logging.error("RUN_FAILED type=%s; no automatic retry; inspect account before restart", type(exc).__name__)
        raise SystemExit(1)

if __name__ == "__main__": main()
