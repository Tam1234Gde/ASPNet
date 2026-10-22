from datetime import datetime, timedelta, timezone
from decimal import Decimal
from uuid import uuid4
from .models import decimal_to_quotation

class TBankClient:
    """Асинхронный thin-adapter над официальным t-tech-investments SDK."""
    def __init__(self, settings):
        from t_tech.invest import AsyncClient
        self.s = settings
        from t_tech.invest.constants import INVEST_GRPC_API_SANDBOX, INVEST_GRPC_API
        self.client = AsyncClient(settings.token, target=INVEST_GRPC_API_SANDBOX if settings.sandbox else INVEST_GRPC_API)
        self.services = None

    async def __aenter__(self):
        self.services = await self.client.__aenter__()
        return self

    async def __aexit__(self, *args):
        return await self.client.__aexit__(*args)

    async def candles(self, instrument_id: str):
        from t_tech.invest import CandleInterval
        return await self.services.market_data.get_candles(
            instrument_id=instrument_id,
            from_=datetime.now(timezone.utc) - timedelta(minutes=30),
            to=datetime.now(timezone.utc),
            interval=CandleInterval.CANDLE_INTERVAL_1_MIN)

    async def order_book(self, instrument_id: str, depth: int):
        return await self.services.market_data.get_order_book(
            instrument_id=instrument_id, depth=depth)

    async def last_price(self, instrument_id: str):
        return await self.services.market_data.get_last_prices(instrument_id=[instrument_id])

    async def trading_status(self, instrument_id: str):
        return await self.services.market_data.get_trading_status(instrument_id=instrument_id)

    async def post_order(self, instrument_id: str, lots: int, buy: bool, price: Decimal | None = None):
        if lots < 1:
            raise ValueError('Количество лотов должно быть положительным')
        if self.s.dry_run or not self.s.sandbox:
            raise RuntimeError('Orders blocked: requires Sandbox and DRY_RUN=false')
        from .control import Control
        if not Control().permitted(self.s, instrument_id):
            raise RuntimeError('Remote control blocks submission')
        from t_tech.invest import OrderDirection, OrderType
        kwargs = dict(instrument_id=instrument_id, quantity=lots,
            direction=OrderDirection.ORDER_DIRECTION_BUY if buy else OrderDirection.ORDER_DIRECTION_SELL,
            account_id=self.s.account_id, order_id=str(uuid4()),
            order_type=OrderType.ORDER_TYPE_LIMIT if price is not None else OrderType.ORDER_TYPE_MARKET)
        if price is not None:
            from t_tech.invest import Quotation
            units, nano = decimal_to_quotation(price)
            kwargs['price'] = Quotation(units=units, nano=nano)
        service = self.services.sandbox if self.s.sandbox else self.services.orders
        method = 'post_sandbox_order' if self.s.sandbox else 'post_order'
        return await getattr(service, method)(**kwargs)

    async def sandbox_pay_in(self, rub: Decimal):
        if not self.s.sandbox: raise RuntimeError('SandboxPayIn is available only in sandbox')
        from t_tech.invest import MoneyValue
        units, nano = decimal_to_quotation(rub)
        return await self.services.sandbox.sandbox_pay_in(
            account_id=self.s.account_id, amount=MoneyValue(currency='rub', units=units, nano=nano))

    async def portfolio(self):
        if self.s.sandbox:
            return await self.services.sandbox.get_sandbox_portfolio(account_id=self.s.account_id)
        return await self.services.operations.get_portfolio(account_id=self.s.account_id)

    async def positions(self):
        if self.s.sandbox:
            return await self.services.sandbox.get_sandbox_positions(account_id=self.s.account_id)
        return await self.services.operations.get_positions(account_id=self.s.account_id)

    async def instrument(self, uid):
        from t_tech.invest import InstrumentIdType
        return (await self.services.instruments.get_instrument_by(
            id_type=(InstrumentIdType.INSTRUMENT_ID_TYPE_FIGI if len(uid) == 12 else InstrumentIdType.INSTRUMENT_ID_TYPE_UID), id=uid)).instrument

    async def order_state(self, order_id):
        service = self.services.sandbox if self.s.sandbox else self.services.orders
        method = 'get_sandbox_order_state' if self.s.sandbox else 'get_order_state'
        return await getattr(service, method)(account_id=self.s.account_id, order_id=order_id)
