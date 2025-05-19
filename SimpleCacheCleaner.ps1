# Windows Cache Cleaner PowerShell Script
# Requires administrator privileges

# Check if running as administrator
if (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Warning "This script requires administrator privileges. Please run PowerShell as an administrator."
    Read-Host "Press Enter to exit"
    exit
}

function Remove-CacheFiles {
    param (
        [string]$path
    )
    
    if (Test-Path $path) {
        Write-Host "Cleaning files in $path"
        try {
            Get-ChildItem -Path $path -File -Force -ErrorAction SilentlyContinue | ForEach-Object {
                try {
                    Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
                } catch {
                    # Ignore errors for individual files
                }
            }

            Get-ChildItem -Path $path -Directory -Force -ErrorAction SilentlyContinue | ForEach-Object {
                try {
                    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
                } catch {
                    # Ignore errors for individual directories
                }
            }
        } catch {
            Write-Warning "Error accessing $path"
        }
    } else {
        Write-Host "Path not found: $path"
    }
}

# Show menu and get user choices
function Show-Menu {
    Clear-Host
    Write-Host "================ Windows Cache Cleaner ================"
    Write-Host "Select cache locations to clean (separate by comma):"
    Write-Host "1. Windows Temp Files"
    Write-Host "2. Windows Prefetch"
    Write-Host "3. Google Chrome Cache"
    Write-Host "4. Mozilla Firefox Cache" 
    Write-Host "5. Microsoft Edge Cache"
    Write-Host "6. All Caches"
    Write-Host "Q. Quit"
    Write-Host "===================================================="

    $choices = Read-Host "Enter your choices (e.g., 1,3,5 or 6 for all)"
    return $choices
}

function Clean-WindowsTempFiles {
    Write-Host "`nCleaning Windows Temp Files..." -ForegroundColor Cyan
    $tempPaths = @(
        $env:TEMP,
        "$env:SystemRoot\Temp"
    )
    
    foreach ($path in $tempPaths) {
        Remove-CacheFiles -path $path
    }
}

function Clean-PrefetchFiles {
    Write-Host "`nCleaning Windows Prefetch..." -ForegroundColor Cyan
    $prefetchPath = "$env:SystemRoot\Prefetch"
    Remove-CacheFiles -path $prefetchPath
}

function Clean-ChromeCache {
    Write-Host "`nCleaning Google Chrome Cache..." -ForegroundColor Cyan
    $chromeCachePath = "$env:LOCALAPPDATA\Google\Chrome\User Data\Default\Cache"
    Remove-CacheFiles -path $chromeCachePath
}

function Clean-FirefoxCache {
    Write-Host "`nCleaning Mozilla Firefox Cache..." -ForegroundColor Cyan
    $firefoxProfilesPath = "$env:APPDATA\Mozilla\Firefox\Profiles"
    
    if (Test-Path $firefoxProfilesPath) {
        Get-ChildItem -Path $firefoxProfilesPath -Directory | ForEach-Object {
            $cachePath = "$($_.FullName)\cache"
            $cache2Path = "$($_.FullName)\cache2"
            
            Remove-CacheFiles -path $cachePath
            Remove-CacheFiles -path $cache2Path
        }
    } else {
        Write-Host "Firefox profiles folder not found."
    }
}

function Clean-EdgeCache {
    Write-Host "`nCleaning Microsoft Edge Cache..." -ForegroundColor Cyan
    $edgeCachePath = "$env:LOCALAPPDATA\Microsoft\Edge\User Data\Default\Cache"
    Remove-CacheFiles -path $edgeCachePath
}

# Main script
$choices = Show-Menu

if ($choices -eq "Q" -or $choices -eq "q") {
    exit
}

$options = $choices -split ',' | ForEach-Object { $_.Trim() }

if ($options -contains "6") {
    Clean-WindowsTempFiles
    Clean-PrefetchFiles
    Clean-ChromeCache
    Clean-FirefoxCache
    Clean-EdgeCache
} else {
    if ($options -contains "1") { Clean-WindowsTempFiles }
    if ($options -contains "2") { Clean-PrefetchFiles }
    if ($options -contains "3") { Clean-ChromeCache }
    if ($options -contains "4") { Clean-FirefoxCache }
    if ($options -contains "5") { Clean-EdgeCache }
}

Write-Host "`nCache cleaning completed!" -ForegroundColor Green
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") 