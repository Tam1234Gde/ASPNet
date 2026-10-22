from dataclasses import dataclass
import os
from dotenv import load_dotenv

load_dotenv()

def _bool(name: str, default: bool) -> bool:
    return os.getenv(name, str(default)).strip().lower() in {"1", "true", "yes", "да"}

def _int(name: str, default: int) -> int:
    return int(os.getenv(name, str(default)))

def _float(name: str, default: float) -> float:
    return float(os.getenv(name, str(default)))

@dataclass(frozen=True)
class Settings:
    token: str = os.getenv("TINKOFF_INVEST_TOKEN", "")
    account_id: str = os.getenv("ACCOUNT_ID", "")
    sandbox: bool = _bool("IS_SANDBOX", True)
    dry_run: bool = _bool("DRY_RUN", True)
    ai_base_url: str = os.getenv("AI_BASE_URL", "https://apiduck.sytes.net")
    ai_provider: str = os.getenv("AI_PROVIDER", "anthropic").lower()
    ai_decision_path: str = os.getenv("AI_DECISION_PATH", "/v1/messages")
    ai_feedback_path: str = os.getenv("AI_FEEDBACK_PATH", "")
    ai_token: str = os.getenv("AI_API_TOKEN", os.getenv("ANTHROPIC_AUTH_TOKEN", ""))
    ai_model: str = os.getenv("AI_MODEL", "claude-sonnet-4-6")
    ai_max_tokens: int = _int("AI_MAX_TOKENS", 512)
    ai_timeout: float = _float("AI_TIMEOUT_SECONDS", 15)
    instrument_uids: tuple[str, ...] = tuple(x for x in os.getenv("INSTRUMENT_UIDS", "").split(",") if x)
    candle_interval: str = os.getenv("CANDLE_INTERVAL", "1min")
    orderbook_depth: int = _int("ORDERBOOK_DEPTH", 20)
    poll_interval: float = _float("POLL_INTERVAL_SECONDS", 5)
    max_drawdown_rub: float = _float("MAX_SESSION_DRAWDOWN_RUB", 10000)
    max_order_lots: int = _int("MAX_ORDER_LOTS", 10)
    allow_market_orders: bool = _bool("ALLOW_MARKET_ORDERS", False)
    sandbox_initial_rub: float = _float("SANDBOX_INITIAL_RUB", 100000)
    journal_path: str = os.getenv("JOURNAL_PATH", "state/orders.sqlite3")
    max_snapshot_age_seconds: float = _float("MAX_SNAPSHOT_AGE_SECONDS", 30)

    def validate(self) -> None:
        if not self.dry_run and (not self.token or not self.account_id):
            raise ValueError("Для торговли нужны TINKOFF_INVEST_TOKEN и ACCOUNT_ID")
        if self.max_drawdown_rub <= 0 or self.max_order_lots <= 0:
            raise ValueError("Лимиты риска должны быть положительными")
