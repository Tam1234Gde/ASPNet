"""Read-only Sandbox diagnostics. Never submits orders or pays in."""
import asyncio
import logging
from datetime import datetime, timezone
from .config import Settings
from .tbank import TBankClient
from .models import quotation_to_decimal as dec

async def inspect_account(s):
    if not s.sandbox:
        raise RuntimeError('Diagnostics requires IS_SANDBOX=true')
    if not s.token or not s.account_id:
        raise ValueError('Token and ACCOUNT_ID required')
    async with TBankClient(s) as broker:
        accounts = await broker.services.sandbox.get_sandbox_accounts()
        if not any(a.id == s.account_id for a in accounts.accounts):
            raise ValueError('ACCOUNT_ID not found among Sandbox accounts')
        portfolio = await broker.portfolio()
        orders = await broker.services.sandbox.get_sandbox_orders(account_id=s.account_id)
        logging.info('ACCOUNT_OK sandbox=true equity=%s %s positions=%s active_orders=%s',
                     dec(portfolio.total_amount_portfolio), portfolio.total_amount_portfolio.currency,
                     len(portfolio.positions), len(orders.orders))
        for p in portfolio.positions:
            logging.info('POSITION uid=%s lots=%s unrealized_pnl=%s (instrument currency)',
                         p.instrument_uid, dec(p.quantity_lots), dec(p.expected_yield))
        for order in orders.orders:
            logging.info('ORDER_STATE id=%s status=%s filled=%s requested=%s', order.order_id,
                         order.execution_report_status.name, order.lots_executed, order.lots_requested)
        for identifier in s.instrument_uids:
            item = await broker.instrument(identifier)
            status = await broker.trading_status(item.uid)
            candles = await broker.candles(item.uid)
            book = await broker.order_book(item.uid, s.orderbook_depth)
            prices = await broker.last_price(item.uid)
            logging.info('MARKET ticker=%s uid=%s lot=%s status=%s api_trade=%s candles=%s bids=%s asks=%s prices=%s',
                         item.ticker, item.uid, item.lot, status.trading_status.name,
                         status.api_trade_available_flag, len(candles.candles), len(book.bids), len(book.asks), len(prices.last_prices))
        logging.info('DIAGNOSTICS_OK time=%s orders_sent=0', datetime.now(timezone.utc).isoformat())

async def run(s):
    await asyncio.wait_for(inspect_account(s), timeout=60)

def main():
    from .observability import setup_logging
    setup_logging()
    try:
        asyncio.run(run(Settings()))
    except Exception as exc:
        logging.error('DIAGNOSTICS_FAILED type=%s; check token, Sandbox ACCOUNT_ID and connectivity', type(exc).__name__)
        raise SystemExit(1)

if __name__ == '__main__':
    main()
