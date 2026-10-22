@echo off
cd /d "%~dp0"
powershell.exe -NoProfile -Command "Get-Content .\state\bot.log -Encoding UTF8 -Tail 40 -Wait"
