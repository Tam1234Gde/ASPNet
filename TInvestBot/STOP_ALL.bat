@echo off
setlocal
cd /d "%~dp0"
echo Stopping Telegram...
call "%~dp0STOP_TELEGRAM.bat"
echo Requesting investment bot stop...
call "%~dp0STOP_BOT.bat"
echo Done. Existing orders and positions were not cancelled.
