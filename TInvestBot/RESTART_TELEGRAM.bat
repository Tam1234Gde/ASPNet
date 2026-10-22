@echo off
setlocal
cd /d "%~dp0"
call "%~dp0STOP_TELEGRAM.bat"
if not "%ERRORLEVEL%"=="0" exit /b %ERRORLEVEL%
timeout /t 2 /nobreak >nul
start "TInvestBot Telegram" /min "%~dp0START_TELEGRAM.bat"
echo Telegram restart requested. Check state\telegram.stdout.log and state\telegram.stderr.log.
exit /b 0
