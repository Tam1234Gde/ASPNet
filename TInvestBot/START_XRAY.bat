@echo off
setlocal
cd /d "%~dp0"
if not exist .env (
  echo ERROR: .env not found
  pause
  exit /b 2
)
for /f "usebackq tokens=1,* delims==" %%A in (`findstr /b "XRAY_PATH=" .env`) do set "XRAY_PATH=%%B"
for /f "usebackq tokens=1,* delims==" %%A in (`findstr /b "VPN_CONFIG_PATH=" .env`) do set "VPN_CONFIG_PATH=%%B"
if not defined XRAY_PATH set "XRAY_PATH=C:\Tools\xray\xray.exe"
if not defined VPN_CONFIG_PATH set "VPN_CONFIG_PATH=state\telegram_xray.json"
if not exist "%XRAY_PATH%" (
  echo ERROR: Xray not found: %XRAY_PATH%
  echo Download the official Windows archive from https://github.com/XTLS/Xray-core/releases
  pause
  exit /b 3
)
if not exist "%VPN_CONFIG_PATH%" (
  echo ERROR: config not found: %VPN_CONFIG_PATH%
  echo Send /vpn_json to the Telegram bot or save the JSON there.
  pause
  exit /b 4
)
"%XRAY_PATH%" run -test -c "%VPN_CONFIG_PATH%"
if errorlevel 1 (
  echo ERROR: invalid Xray config
  pause
  exit /b 5
)
echo Starting Xray. Telegram should use http://127.0.0.1:10809; T-Invest stays direct.
"%XRAY_PATH%" run -c "%VPN_CONFIG_PATH%" >> state\xray.stdout.log 2>> state\xray.stderr.log
