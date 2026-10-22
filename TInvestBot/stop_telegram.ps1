$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

$processes = @(Get-CimInstance Win32_Process -Filter "Name = 'python.exe' OR Name = 'pythonw.exe'" |
    Where-Object { $_.CommandLine -match '(-m\s+app\.telegram|app\\telegram\.py)' })

if ($processes.Count -eq 0) {
    Write-Output 'Telegram bot is not running.'
    exit 0
}

foreach ($item in $processes) {
    Write-Output ("Stopping Telegram PID=" + $item.ProcessId)
    Stop-Process -Id $item.ProcessId -Force -ErrorAction SilentlyContinue
}

$deadline = (Get-Date).AddSeconds(15)
do {
    Start-Sleep -Milliseconds 300
    $locked = @(Get-CimInstance Win32_Process -Filter "Name = 'python.exe' OR Name = 'pythonw.exe'" |
        Where-Object { $_.CommandLine -match '(-m\s+app\.telegram|app\\telegram\.py)' })
} while ($locked.Count -gt 0 -and (Get-Date) -lt $deadline)

if ($locked.Count -gt 0) { throw 'Telegram process did not stop within 15 seconds.' }
Write-Output 'Telegram stopped. State and databases were preserved.'
