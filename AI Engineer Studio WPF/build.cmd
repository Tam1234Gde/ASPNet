@echo off
setlocal
cd /d "%~dp0"
dotnet restore "AIEngineerStudio.Wpf.csproj" --force-evaluate --ignore-failed-sources
if errorlevel 1 exit /b %errorlevel%
dotnet build "AIEngineerStudio.Wpf.slnx" --configuration Debug --no-restore
exit /b %errorlevel%
