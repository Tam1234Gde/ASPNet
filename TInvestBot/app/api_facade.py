"""Высокоуровневый безопасный фасад T-Invest API для Telegram.
Чтение доступно в sandbox и production SDK, изменения вынесены в явные методы.
"""
from __future__ import annotations
import asyncio
import logging
from datetime import datetime, timedelta, timezone
from .models import encode

class BrokerFacade:
    def __init__(self, broker, retries=2): self.b=broker; self.retries=retries
    async def _read(self, fn, **kwargs):
        last=None
        for attempt in range(self.retries+1):
            try: return await fn(**kwargs)
            except Exception as exc:
                last=exc
                if attempt == self.retries: break
                logging.warning('TBANK_READ_RETRY method=%s attempt=%s type=%s',getattr(fn,'__name__','api'),attempt+1,type(exc).__name__)
                await asyncio.sleep(0.3*(2**attempt))
        raise last
    async def accounts(self):
        if self.b.s.sandbox: return encode(await self._read(self.b.services.sandbox.get_sandbox_accounts))
        return encode(await self._read(self.b.services.users.get_accounts))
    async def user_info(self): return encode(await self._read(self.b.services.users.get_info))
    async def tariff(self): return encode(await self._read(self.b.services.users.get_user_tariff))
    async def margin(self): return encode(await self._read(self.b.services.users.get_margin_attributes,account_id=self.b.s.account_id))
    async def withdraw_limits(self):
        service=self.b.services.sandbox if self.b.s.sandbox else self.b.services.operations
        method=service.get_sandbox_withdraw_limits if self.b.s.sandbox else service.get_withdraw_limits
        return encode(await self._read(method,account_id=self.b.s.account_id))
    async def operations(self, days=30):
        now=datetime.now(timezone.utc); service=self.b.services.sandbox if self.b.s.sandbox else self.b.services.operations
        method=service.get_sandbox_operations if self.b.s.sandbox else service.get_operations
        return encode(await self._read(method,account_id=self.b.s.account_id,from_=now-timedelta(days=days),to=now))
    async def market(self, instrument_id):
        md=self.b.services.market_data
        last,book,status=await asyncio.gather(
            self._read(md.get_last_prices,instrument_id=[instrument_id]),
            self._read(md.get_order_book,instrument_id=instrument_id,depth=10),
            self._read(md.get_trading_status,instrument_id=instrument_id))
        return {'last_prices':encode(last),'order_book':encode(book),'trading_status':encode(status)}
    async def candles(self, instrument_id, hours=24):
        from t_tech.invest import CandleInterval
        now=datetime.now(timezone.utc)
        return encode(await self._read(self.b.services.market_data.get_candles,instrument_id=instrument_id,
                         from_=now-timedelta(hours=min(max(hours,1),168)),to=now,
                         interval=CandleInterval.CANDLE_INTERVAL_HOUR))
    async def instrument(self, instrument_id): return encode(await self.b.instrument(instrument_id))
    async def search(self, query): return encode(await self._read(self.b.services.instruments.find_instrument,query=query))
    async def dividends(self, instrument_id, days=365):
        now=datetime.now(timezone.utc)
        return encode(await self._read(self.b.services.instruments.get_dividends,instrument_id=instrument_id,
                                        from_=now-timedelta(days=30),to=now+timedelta(days=days)))
    async def coupons(self, instrument_id, days=365):
        now=datetime.now(timezone.utc)
        return encode(await self._read(self.b.services.instruments.get_bond_coupons,instrument_id=instrument_id,
                                        from_=now-timedelta(days=30),to=now+timedelta(days=days)))
    async def schedules(self, exchange=''):
        now=datetime.now(timezone.utc)
        return encode(await self._read(self.b.services.instruments.trading_schedules,exchange=exchange,
                                        from_=now,to=now+timedelta(days=7)))
    async def favorites(self): return encode(await self._read(self.b.services.instruments.get_favorites))
    async def countries(self): return encode(await self._read(self.b.services.instruments.get_countries))
    async def stop_orders(self):
        if self.b.s.sandbox:
            from t_tech.invest import GetStopOrdersRequest
            return encode(await self._read(self.b.services.sandbox.get_sandbox_stop_orders,
                                            request=GetStopOrdersRequest(account_id=self.b.s.account_id)))
        return encode(await self._read(self.b.services.stop_orders.get_stop_orders,account_id=self.b.s.account_id))
