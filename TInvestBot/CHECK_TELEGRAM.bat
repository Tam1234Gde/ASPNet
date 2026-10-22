@echo off
setlocal
cd /d "%~dp0"
if not exist .venv314\Scripts\python.exe (
  echo ERROR: .venv314 not found
  exit /b 3
)
".venv314\Scripts\python.exe" -c "import os,sys; from dotenv import load_dotenv; load_dotenv(); print('TOKEN:', bool(os.getenv('TELEGRAM_BOT_TOKEN'))); print('USER_IDS:', os.getenv('TELEGRAM_ALLOWED_USER_IDS','')); print('XRAY_PATH:', os.getenv('XRAY_PATH','')); print('VPN_CONFIG_PATH:', os.getenv('VPN_CONFIG_PATH','state/telegram_xray.json')); print('TELEGRAM_PROXY:', os.getenv('TELEGRAM_PROXY',''))"
echo.
echo Logs:
echo state\telegram.stderr.log
echo state\xray.stderr.log
pause
