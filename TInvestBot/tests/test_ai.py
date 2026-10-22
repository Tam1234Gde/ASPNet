import asyncio
import json
from dataclasses import replace
import httpx
import pytest
from app.ai import AIClient
from app.config import Settings
from app.main import run_once


def test_http_contract_and_feedback():
    async def check():
        ai = AIClient(replace(Settings(), ai_token='test-key'))
        await ai.http.aclose()
        requests = []
        def handler(request):
            requests.append(json.loads(request.content))
            return httpx.Response(200, json={'content': [{'type': 'text', 'text': '{"ACTION":"HOLD","INSTRUMENT_ID":"x","LOTS":0,"CONFIDENCE":0}'}]})
        ai.http = httpx.AsyncClient(base_url='https://example.test', transport=httpx.MockTransport(handler))
        try:
            await ai.feedback({'order_id': '123'})
            d = await ai.decide({'instruments': [{'instrument_id': 'x'}]})
            assert d.action.value == 'HOLD'
            payload = requests[-1]
            assert payload['model'] == ai.s.ai_model
            assert payload['temperature'] == 0
            assert json.loads(payload['messages'][0]['content'])['recent_feedback'][0]['order_id'] == '123'
        finally:
            await ai.close()
    asyncio.run(check())

@pytest.mark.parametrize('text', ['nonsense', '{"ACTION":"BUY","INSTRUMENT_ID":"x","LOTS":true,"CONFIDENCE":1}', '{"ACTION":"BUY","INSTRUMENT_ID":"x","LOTS":1,"CONFIDENCE":2}'])
def test_invalid_response(text):
    from app.models import Directive
    with pytest.raises(ValueError):
        Directive.from_json(AIClient._json_from_response({'content': [{'text': text}]}))

def test_production_blocked():
    with pytest.raises(RuntimeError, match='Production blocked'):
        asyncio.run(run_once(replace(Settings(), dry_run=False, sandbox=False)))

def test_dry_run_no_network():
    asyncio.run(run_once(replace(Settings(), dry_run=True)))
