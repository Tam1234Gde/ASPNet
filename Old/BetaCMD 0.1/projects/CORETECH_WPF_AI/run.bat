@echo off
echo ========================================
echo CORETECH IT DEV - Starting Application
echo ========================================
echo.

if not exist "AISoftCraftDuck\bin\Debug\net10.0-windows\CORETECH_IT_DEV.exe" (
    echo Application not built yet. Building now...
    call build.bat
    if errorlevel 1 exit /b 1
)

cd AISoftCraftDuck\bin\Debug\net10.0-windows

echo Starting CORETECH IT DEV...
start CORETECH_IT_DEV.exe

cd ..\..\..\..\

echo.
echo Application started!
echo.
