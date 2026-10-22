@echo off
cd /d "%~dp0"
echo Starting continuous SANDBOX trading. Real-money trading is blocked.
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0start_bot.ps1"
pause
