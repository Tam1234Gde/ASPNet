@echo off
chcp 65001 > nul
echo 🚀 Запуск автономного ИИ-агента (Claude)...

set CVC_API_KEY=sk-cvc-1b6c31ea59c38507badec70ead406011503a560a9a8c76ec373cfc0ddb58587e

:: Задаем HTTP-прокси вместо SOCKS4
set HTTP_PROXY=http://127.0.0.1:10809
set HTTPS_PROXY=http://127.0.0.1:10809
set http_proxy=http://127.0.0.1:10809
set https_proxy=http://127.0.0.1:10809

python agent.py

pause