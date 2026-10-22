from pathlib import Path
import re
from html.parser import HTMLParser
class Text(HTMLParser):
    def handle_data(self, data):
        if any(x in data.lower() for x in ('anthropic','https://','model','feedback','authorization','/v1/')):
            print(re.sub(r'[A-Za-z0-9_./+=-]{40,}', '[REDACTED]', data)[:1500])
Text().feed(next(Path('res').glob('*.html')).read_text(encoding='utf-8'))
import inspect
from t_tech.invest import schemas as s, async_services as a
for name in ('TradingDay','Instrument','PortfolioResponse','PositionsResponse','OrderState','MarketDataRequest','SubscribeCandlesRequest','CandleInstrument','OrderBookInstrument','TradeInstrument'):
    print(name, inspect.signature(getattr(s,name)))
for name in ('SandboxService','InstrumentsService','MarketDataStreamService'):
    cls=getattr(a,name)
    for method in ('trading_schedules','get_sandbox_positions','get_sandbox_orders','cancel_sandbox_order','market_data_stream'):
        if hasattr(cls,method): print(name,method,inspect.signature(getattr(cls,method)))
