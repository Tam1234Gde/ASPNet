"""Explicit online AI check; never connects to broker or submits orders."""
import asyncio
from datetime import datetime, timezone
from .ai import AIClient
from .config import Settings
from .models import MarketSnapshot

async def main():
    s = Settings()
    if not s.ai_token or s.ai_token == 'replace_me':
        raise SystemExit('Set AI_API_TOKEN in .env')
    ai = AIClient(s)
    try:
        d = await ai.decide(MarketSnapshot(datetime.now(timezone.utc).isoformat(),
                           [{"instrument_id": "connectivity-test", "test_only": True}]))
        print('AI connection and JSON validation OK. Action:', d.action.value)
    finally:
        await ai.close()

if __name__ == '__main__': asyncio.run(main())
