@echo off
setlocal
cd /d "%~dp0"
if not exist state mkdir state
start "TInvest Telegram" /min cmd /c call "%~dp0START_TELEGRAM.bat"
start "TInvest Sandbox Daemon" /min cmd /c call "%~dp0START_MANAGED_BOT.bat"
start "TInvest Monitor" cmd /k call "%~dp0WATCH_ALL.bat"
echo Telegram, Sandbox daemon and monitor started in separate windows.
