"""Проверка доступного лимита токенов API Duck."""
from __future__ import annotations
import httpx

URL = "https://apiduck.servepics.com/v1/messages/count_tokens"

async def count_tokens(*, model: str, messages: list, system: str = "", timeout: float = 10.0) -> dict:
    payload = {"model": model, "messages": messages}
    if system:
        payload["system"] = system
    async with httpx.AsyncClient(timeout=timeout) as client:
        response = await client.post(URL, json=payload)
        response.raise_for_status()
        data = response.json()
    if not isinstance(data, dict):
        raise ValueError("count_tokens вернул некорректный ответ")
    return data
