@echo off
echo ========================================
echo CORETECH IT DEV - Build Script
echo ========================================
echo.

cd AISoftCraftDuck

echo [1/3] Restoring NuGet packages...
dotnet restore CORETECH_IT_DEV.csproj
if errorlevel 1 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo.
echo [2/3] Building project...
dotnet build CORETECH_IT_DEV.csproj --configuration Debug --no-restore
if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo [3/3] Build completed successfully!
echo.
echo ========================================
echo Output: AISoftCraftDuck\bin\Debug\net10.0-windows\CORETECH_IT_DEV.exe
echo ========================================
echo.
echo To run the application, use: run.bat
echo Or manually: cd AISoftCraftDuck\bin\Debug\net10.0-windows
echo              CORETECH_IT_DEV.exe
echo.

cd ..
pause
