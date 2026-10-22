$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot
Set-Content -Path '.\state\stop.request' -Value 'stop' -Encoding UTF8
Write-Output 'Stop requested. Allow up to AI timeout + 120 seconds. Existing broker orders/positions remain open. Check state\status.json for stopped.'
