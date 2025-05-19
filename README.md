# Windows Cache Cleaner

A simple utility to clean temporary files and caches from your Windows system to improve performance.

## Features

- Cleans Windows temporary files
- Cleans Windows prefetch files
- Cleans browser caches:
  - Google Chrome
  - Mozilla Firefox
  - Microsoft Edge
- Requires administrator privileges to access protected system folders

## Requirements

- Windows 10/11
- .NET 6.0 Runtime or later

## Installation

1. Make sure you have the [.NET 6.0 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0) installed
2. Download the latest release from the Releases section
3. Extract the ZIP file to any location on your computer
4. Run WindowsCacheCleaner.exe

## Building from Source

1. Install the [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Clone this repository
3. Open a command prompt in the repository folder
4. Run `dotnet build` to build the application
5. Run `dotnet publish -c Release -r win-x64 --self-contained false` to create a distributable version

## Usage

1. Launch the application (right-click and select "Run as administrator")
2. Select the types of cache you want to clean
3. Click "Clean Now" to start the cleaning process
4. Wait for the process to complete
5. Click "Exit" to close the application

## Warning

This application deletes files from your system. While it's designed to only remove temporary files and caches, please use it at your own risk. Always ensure you have backups of important data.

## License

MIT License 