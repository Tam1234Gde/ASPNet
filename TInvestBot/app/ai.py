"""Клиент удалённого AI DUCK (Anthropic-compatible API).

Локально не выполняются ML/TA вычисления: клиент только сериализует snapshot,
передаёт его модели и валидирует структурированный ответ.
"""
import json
import re
from typing import Any
import httpx
from .models import Directive, encode
from .config import Settings
from .learning import LearningStore

SYSTEM_PROMPT = """Ты — внешний decision engine торгового thin-client. Локальный клиент не
строит стратегию и не меняет твой ответ. Проанализируй переданный snapshot и верни
ТОЛЬКО JSON без markdown строго с полями ACTION (BUY|SELL|HOLD|CLOSE),
INSTRUMENT_ID (uid), LOTS (целое), PRICE_LIMIT (строка или null), CONFIDENCE (0..1).
Не выдумывай отсутствующие данные; при недостатке данных верни HOLD с LOTS=0.
"""

class AIProtocolError(ValueError):
    pass

class AIClient:
    def __init__(self, settings: Settings):
        self.s = settings
        self.feedback_context = []
        self.learning = LearningStore()
        headers = {"Authorization": f"Bearer {settings.ai_token}"} if settings.ai_token else {}
        if settings.ai_provider == "anthropic":
            headers.update({"anthropic-version": "2023-06-01", "x-api-key": settings.ai_token})
        self.http = httpx.AsyncClient(base_url=settings.ai_base_url.rstrip("/"), headers=headers,
                                      timeout=settings.ai_timeout)

    @staticmethod
    def _json_from_response(data: Any) -> dict:
        # Поддержка Anthropic Messages, OpenAI-compatible и прямого JSON.
        if isinstance(data, dict) and all(k in data for k in ("ACTION", "LOTS", "CONFIDENCE")):
            return data
        text = None
        if isinstance(data, dict) and isinstance(data.get("content"), list):
            text = "".join(x.get("text", "") for x in data["content"] if isinstance(x, dict))
        if text is None and isinstance(data, dict) and isinstance(data.get("choices"), list):
            text = data["choices"][0].get("message", {}).get("content", "")
        if text is None and isinstance(data, str): text = data
        if not text: raise AIProtocolError("AI response has no text content")
        text = re.sub(r"^```(?:json)?\s*|\s*```$", "", text.strip(), flags=re.I)
        try: return json.loads(text)
        except json.JSONDecodeError as e: raise AIProtocolError(f"AI returned invalid JSON: {e}") from e

    async def decide(self, snapshot):
        payload = {"model": self.s.ai_model, "max_tokens": self.s.ai_max_tokens,
                   "temperature": 0, "system": SYSTEM_PROMPT,
                   "messages": [{"role": "user", "content": json.dumps({"snapshot": encode(snapshot), "risk_limits": {"max_order_lots": self.s.max_order_lots, "max_position_lots": self.s.max_order_lots, "allow_market_orders": self.s.allow_market_orders, "short_selling": False, "instruction": "Respect all limits; return HOLD if unable."}, "recent_feedback": self.feedback_context}, ensure_ascii=False)}]}
        r = await self.http.post(self.s.ai_decision_path, json=payload)
        r.raise_for_status()
        directive = Directive.from_json(self._json_from_response(r.json()))
        self.learning.record(action=directive.action.value, instrument=directive.instrument_id, confidence=str(directive.confidence), source='ai_decision')
        return directive

    async def feedback(self, feedback):
        self.feedback_context.append(encode(feedback))
        self.feedback_context = self.feedback_context[-20:]
        if not self.s.ai_feedback_path:
            return
        # Для DUCK feedback — отдельный user message на совместимом endpoint.
        payload = {"model": self.s.ai_model, "max_tokens": self.s.ai_max_tokens,
                   "temperature": 0, "system": "Зафиксируй post-trade telemetry для последующего контекста. Не принимай торговое решение.",
                   "messages": [{"role": "user", "content": json.dumps(encode(feedback), ensure_ascii=False)}]}
        r = await self.http.post(self.s.ai_feedback_path, json=payload)
        r.raise_for_status()

    async def close(self):
        await self.http.aclose()
