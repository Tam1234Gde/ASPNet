@echo off
setlocal
cd /d "%~dp0"
if not exist state mkdir state
if not exist .env (
  echo ERROR: .env not found
  exit /b 2
)
if not exist .venv314\Scripts\python.exe (
  echo ERROR: Python environment .venv314 not found
  exit /b 3
)
echo Starting Telegram bot. Logs: state\telegram.stdout.log, state\telegram.stderr.log
".venv314\Scripts\python.exe" -m app.telegram >> state\telegram.stdout.log 2>> state\telegram.stderr.log
set ERR=%ERRORLEVEL%
echo Telegram stopped with code %ERR%
if not "%ERR%"=="0" echo Check state\telegram.stderr.log and state\xray.stderr.log
exit /b %ERR%
