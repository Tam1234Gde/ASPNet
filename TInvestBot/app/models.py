from dataclasses import dataclass, asdict, is_dataclass
from decimal import Decimal
from enum import Enum
from datetime import datetime
from typing import Any

class Action(str, Enum):
    BUY = 'BUY'
    SELL = 'SELL'
    HOLD = 'HOLD'
    CLOSE = 'CLOSE'

@dataclass(frozen=True)
class Directive:
    action: Action
    instrument_id: str
    lots: int
    price_limit: Decimal | None
    confidence: Decimal

    @classmethod
    def from_json(cls, data: dict[str, Any]):
        if not isinstance(data, dict): raise ValueError('Expected JSON object')
        action = Action(data['ACTION'])
        lots = data['LOTS']
        if type(lots) is not int or lots < 0: raise ValueError('LOTS must be nonnegative integer')
        instrument = data['INSTRUMENT_ID']
        if not isinstance(instrument, str): raise ValueError('Invalid instrument')
        price_value = data.get('PRICE_LIMIT')
        price = None if price_value is None else Decimal(str(price_value))
        confidence = Decimal(str(data['CONFIDENCE']))
        if not confidence.is_finite() or not 0 <= confidence <= 1: raise ValueError('Invalid confidence')
        if price is not None and (not price.is_finite() or price <= 0): raise ValueError('Invalid price')
        if action != Action.HOLD and (lots <= 0 or not instrument): raise ValueError('Empty trade')
        return cls(action, instrument, lots, price, confidence)

def encode(value):
    if isinstance(value, Decimal): return str(value)
    if isinstance(value, datetime): return value.isoformat()
    if isinstance(value, Enum): return value.name
    if is_dataclass(value): return encode(asdict(value))
    if isinstance(value, dict): return {k: encode(v) for k, v in value.items()}
    if isinstance(value, (list, tuple)): return [encode(v) for v in value]
    return value

@dataclass
class MarketSnapshot:
    timestamp: str
    instruments: list[dict[str, Any]]
    orderbooks: dict[str, Any] = None
    candles: dict[str, Any] = None
    positions: list[dict[str, Any]] = None
    portfolio: dict[str, Any] = None

@dataclass
class TradeFeedback:
    directive: dict[str, Any]
    realized_pnl_rub: Decimal
    commission_rub: Decimal
    slippage_rub: Decimal
    execution_delay_ms: int
    market_context: dict[str, Any]

def quotation_to_decimal(value):
    return Decimal(value.units) + Decimal(value.nano) / Decimal(1_000_000_000)

def decimal_to_quotation(value):
    value = Decimal(str(value))
    if not value.is_finite() or value != value.quantize(Decimal('.000000001')):
        raise ValueError('Value must be finite and have at most 9 decimal places')
    units = int(value)
    if not -(2**63) <= units < 2**63: raise ValueError('int64 overflow')
    return units, int((value - units) * 1_000_000_000)
