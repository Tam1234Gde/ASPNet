param([switch]$Observe)
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot
$python = Join-Path $PSScriptRoot '.venv\Scripts\python.exe'
& $python -c "from app.runtime import ProcessLock; lock=ProcessLock(); lock.__enter__(); lock.__exit__()"
if ($LASTEXITCODE -ne 0) { throw 'Bot already running or lock unavailable' }
$arguments = '-m app.daemon --interval 60 --resume'
if (-not $Observe) { $arguments += ' --sandbox-trade' }
$stamp = Get-Date -Format yyyyMMdd_HHmmss_fff
$process = Start-Process -FilePath $python -ArgumentList $arguments -WorkingDirectory $PSScriptRoot -WindowStyle Hidden -RedirectStandardOutput ".\state\daemon.$stamp.stdout.log" -RedirectStandardError ".\state\daemon.$stamp.stderr.log" -PassThru
Start-Sleep -Seconds 3
$process.Refresh()
if ($process.HasExited) { throw "Worker exited; inspect state\daemon.$stamp.stderr.log" }
Write-Output ('Started worker PID=' + $process.Id + '; SANDBOX ONLY. Monitor state\bot.log. Stop with STOP_BOT.bat')
