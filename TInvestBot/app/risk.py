from dataclasses import dataclass
from decimal import Decimal
from .models import Action

@dataclass
class CircuitBreaker:
    """Сессионный локальный предохранитель. Экземпляр живёт всю торговую сессию."""
    max_drawdown: Decimal
    session_start_equity: Decimal
    tripped: bool = False

    def update(self, equity: Decimal) -> bool:
        equity = Decimal(str(equity))
        if equity.is_finite() and self.session_start_equity - equity >= self.max_drawdown:
            self.tripped = True
        return self.tripped

    def validate(self, d, *, lot_size, price_increment, position_lots=0,
                 max_lots=1, allow_market=False):
        if self.tripped:
            raise RuntimeError("CIRCUIT_BREAKER_TRIPPED")
        if d.action == Action.HOLD:
            return
        if lot_size <= 0 or price_increment <= 0:
            raise ValueError("Invalid instrument metadata")
        if not 0 < d.lots <= max_lots:
            raise ValueError("Order lot ceiling exceeded")
        position_lots = Decimal(str(position_lots))
        if d.action == Action.SELL and d.lots > position_lots:
            raise ValueError("Short selling disabled")
        if d.action == Action.CLOSE and d.lots != abs(position_lots):
            raise ValueError("CLOSE must match full position")
        if d.price_limit is None and not allow_market:
            raise ValueError("Market orders disabled")
        if d.price_limit is not None and d.price_limit % price_increment != 0:
            raise ValueError("Invalid price step")
