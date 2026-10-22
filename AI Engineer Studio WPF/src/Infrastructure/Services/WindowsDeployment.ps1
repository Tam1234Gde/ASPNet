param([string]$OperationDirectory)
$ErrorActionPreference = 'Stop'
$p = Get-Content -LiteralPath (Join-Path $OperationDirectory 'plan.json') -Raw | ConvertFrom-Json
function Log([string]$text) { Add-Content -LiteralPath (Join-Path $OperationDirectory 'journal.log') -Value ((Get-Date).ToUniversalTime().ToString('o') + ' ' + $text) }
function SafePath([string]$root, [string]$relative) {
    if (!$relative -or $relative -match '(^[/\\]|:|(^|[/\\])\.\.([/\\]|$))') { throw 'Unsafe relative path' }
    $full = [IO.Path]::GetFullPath((Join-Path $root $relative))
    if (!$full.StartsWith($root.TrimEnd('\','/') + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Path escape' }
    $part = $full
    while ($part) {
        if (Test-Path -LiteralPath $part) { if ((Get-Item -Force -LiteralPath $part).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'Reparse point rejected' } }
        $part = Split-Path -Parent $part
    }
    return $full
}
function Excluded([string]$rel) {
    foreach ($pattern in $p.Exclusions) { foreach ($part in ($rel -split '[/\\]')) { if ($part -like $pattern) { return $true } } }
    return $false
}
function Files([string]$dir) {
    foreach ($item in Get-ChildItem -LiteralPath $dir -Force) {
        $rel = $item.FullName.Substring($p.Root.Length).TrimStart('\','/')
        if (Excluded $rel) { continue }
        if ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'Reparse point rejected' }
        if ($item.PSIsContainer) { Files $item.FullName } else { $item }
    }
}
function State { if ($p.Manager -eq 'WindowsService') { return (Get-Service -Name $p.Application).Status.ToString() }; return (Get-WebAppPoolState -Name $p.Application).Value }
function WaitState([string]$expected) {
    $deadline = (Get-Date).AddSeconds($p.Timeout)
    while ((State) -ne $expected) { if ((Get-Date) -gt $deadline) { throw 'Application state timeout' }; Start-Sleep -Milliseconds 500 }
}
function StopApp {
    Log 'Остановка'
    if ($p.Manager -eq 'WindowsService') { Stop-Service -Name $p.Application; WaitState 'Stopped' }
    else { if ((State) -ne 'Stopped') { Stop-WebAppPool -Name $p.Application }; WaitState 'Stopped' }
}
function StartApp {
    Log 'Запуск'
    if ($p.Manager -eq 'WindowsService') { Start-Service -Name $p.Application; WaitState 'Running' }
    else { if ((State) -ne 'Started') { Start-WebAppPool -Name $p.Application }; WaitState 'Started' }
    Log 'Проверка успешна'
}
$lock = $null; $stopping = $false; $backed = $false; $mutating = $false; $committed = $false
$backup = Join-Path $p.State ('backup-' + $p.Operation)
$pending = Join-Path $p.State 'pending.json'
try {
    $lock = [IO.File]::Open((Join-Path $p.State 'deployment.lock'), 'OpenOrCreate', 'ReadWrite', 'None')
    if (Test-Path -LiteralPath $pending) { throw 'Unfinished deployment: manual recovery required' }
    if ($p.Manager -eq 'IisApplicationPool') { Import-Module WebAdministration }
    if ($p.Manager -notin @('WindowsService','IisApplicationPool')) { throw 'Unsupported manager' }
    $initial = State
    foreach ($rel in @($p.Upload) + @($p.Delete)) { $null = SafePath $p.Root $rel; if (Excluded $rel) { throw 'Excluded target' } }
    $manifest = Join-Path $p.State 'manifest.json'
    $current = if (Test-Path -LiteralPath $manifest) { Get-Content -LiteralPath $manifest -Raw } else { '' }
    if ($current -cne $p.PreviousManifest) { throw 'Manifest changed concurrently' }
    [IO.File]::WriteAllText($pending, $p.Operation)
    $stopping = $true; StopApp
    Log 'Резервное копирование'
    $null = New-Item -ItemType Directory -Path $backup
    foreach ($file in Files $p.Root) {
        $rel = $file.FullName.Substring($p.Root.Length).TrimStart('\','/')
        $dest = SafePath $backup $rel
        $null = New-Item -ItemType Directory -Force -Path (Split-Path -Parent $dest)
        [IO.File]::Copy($file.FullName, $dest, $false)
    }
    $backed = $true
    $mutating = $true
    foreach ($rel in $p.Upload) {
        Log ('Синхронизация ' + $rel)
        $target = SafePath $p.Root $rel
        $null = New-Item -ItemType Directory -Force -Path (Split-Path -Parent $target)
        [IO.File]::Copy((SafePath (Join-Path $OperationDirectory 'files') $rel), $target, $true)
    }
    foreach ($rel in $p.Delete) { Log ('Удаление ' + $rel); [IO.File]::Delete((SafePath $p.Root $rel)) }
    StartApp
    $temp = Join-Path $p.State ('manifest-' + $p.Operation + '.tmp')
    [IO.File]::WriteAllText($temp, $p.NextManifest)
    if (Test-Path -LiteralPath $manifest) { [IO.File]::Replace($temp, $manifest, $null) } else { [IO.File]::Move($temp, $manifest) }
    $committed = $true
    [IO.File]::WriteAllText((Join-Path $backup '.coretech-complete'), $p.Operation)
    [IO.File]::Delete($pending)
    Log 'Завершено'
    # Only completed backups in this identity-scoped state directory are eligible.
    try {
        Get-ChildItem -LiteralPath $p.State -Directory -Filter 'backup-*' | Where-Object { Test-Path -LiteralPath (Join-Path $_.FullName '.coretech-complete') } | Sort-Object Name -Descending | Select-Object -Skip 5 | ForEach-Object { Remove-Item -LiteralPath $_.FullName -Recurse -Force }
    } catch { Log 'Ошибка очистки старых копий; обновление завершено' }
    exit 0
} catch {
    Log ('Ошибка: ' + $_.Exception.GetType().Name)
    if ($committed) { Log 'Обновление зафиксировано; требуется проверка служебного состояния'; exit 3 }
    if ($stopping) {
        try {
            Log 'Откат'
            if ($mutating -and $backed) {
                StopApp
                foreach ($rel in @($p.Upload) + @($p.Delete)) {
                    $source = SafePath $backup $rel; $target = SafePath $p.Root $rel
                    if (Test-Path -LiteralPath $source) { [IO.File]::Copy($source, $target, $true) }
                    elseif ($rel -in $p.Upload) { [IO.File]::Delete($target) }
                }
            }
            StartApp
            [IO.File]::Delete($pending)
            Log 'Откат и повторный запуск успешны'
        } catch { Log 'Откат не завершён; требуется ручное восстановление'; exit 3 }
    }
    exit 2
} finally { if ($lock) { $lock.Dispose() } }
