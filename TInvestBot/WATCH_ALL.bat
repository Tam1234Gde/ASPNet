@echo off
setlocal
cd /d "%~dp0"
:loop
cls
echo ===== TInvestBot monitor =====
echo.
echo --- processes ---
tasklist /FI "IMAGENAME eq python.exe" /FO TABLE | findstr /I "python.exe" || echo Python processes not found.
echo.
echo --- status ---
if exist state\status.json type state\status.json
echo.
echo --- last Telegram log ---
if exist state\telegram.stderr.log powershell -NoProfile -Command "Get-Content 'state\telegram.stderr.log' -Tail 8"
echo.
echo Refresh: 10 sec. Press Ctrl+C to exit monitor.
timeout /t 10 /nobreak >nul
goto loop
