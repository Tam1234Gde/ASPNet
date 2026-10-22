@echo off
setlocal
cd /d "%~dp0"
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0stop_telegram.ps1"
set ERR=%ERRORLEVEL%
if not "%ERR%"=="0" echo ERROR: Telegram stop failed.
exit /b %ERR%
