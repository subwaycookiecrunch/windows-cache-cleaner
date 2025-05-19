@echo off
echo Windows Cache Cleaner
echo =====================
echo.

:: Check for administrative privileges
net session >nul 2>&1
if %errorLevel% == 0 (
    echo Running with administrator privileges...
) else (
    echo This script requires administrator privileges.
    echo Requesting elevated permissions...
    
    powershell -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

echo Starting Windows Cache Cleaner...
powershell -ExecutionPolicy Bypass -File "%~dp0SimpleCacheCleaner.ps1" 