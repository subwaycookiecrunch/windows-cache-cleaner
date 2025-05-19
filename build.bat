@echo off
echo Building Windows Cache Cleaner...

where dotnet >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo .NET SDK not found. Please install it from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo Building debug version...
dotnet build

echo Building release version...
dotnet publish -c Release -r win-x64 --self-contained false

echo Done!
echo You can find the executable in bin\Release\net6.0-windows\win-x64\publish\
pause 