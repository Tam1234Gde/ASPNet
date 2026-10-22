@echo off
setlocal
cd /d "%~dp0"
if not exist state mkdir state
rem Sandbox trading is explicitly enabled; production remains blocked in app.daemon.
rem --resume clears the persistent stop request after acquiring the lock.
"%~dp0.venv314\Scripts\python.exe" -m app.daemon --sandbox-trade --resume --interval 60 >> "%~dp0state\daemon.stdout.log" 2>> "%~dp0state\daemon.stderr.log"
set "RC=%ERRORLEVEL%"
echo Daemon stopped with code %RC%.
exit /b %RC%
