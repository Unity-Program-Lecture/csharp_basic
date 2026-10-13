param([int]$TimeoutSeconds = 75)

$ErrorActionPreference = 'Stop'
$ngoProjectPath = Split-Path -Parent $PSScriptRoot
$ngoExecutable = Join-Path $ngoProjectPath 'Builds/Windows/NGO.exe'
if (-not (Test-Path -LiteralPath $ngoExecutable)) {
    throw '먼저 README의 Windows 빌드 절차를 진행하세요.'
}

# 각 실행의 결과를 따로 보존합니다. Artifacts는 Git에서 제외됩니다.
$ngoResultPath = Join-Path $ngoProjectPath ('Artifacts/Smoke-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $ngoResultPath | Out-Null
$ngoProcesses = @()
try {
    foreach ($ngoRole in @('host', 'client')) {
        $ngoReportPath = Join-Path $ngoResultPath ($ngoRole + '.json')
        $ngoLogPath = Join-Path $ngoResultPath ($ngoRole + '.log')
        $ngoArguments = '--ngo-smoke-role {0} --ngo-smoke-output "{1}" -logFile "{2}"' -f $ngoRole, $ngoReportPath, $ngoLogPath
        $ngoProcesses += Start-Process -FilePath $ngoExecutable -ArgumentList $ngoArguments -WindowStyle Hidden -PassThru
        if ($ngoRole -eq 'host') { Start-Sleep -Milliseconds 1000 }
    }
    $ngoDeadline = [DateTime]::UtcNow.AddSeconds($TimeoutSeconds)
    $ngoHostFile = Join-Path $ngoResultPath 'host.json'
    $ngoClientFile = Join-Path $ngoResultPath 'client.json'
    while (-not ((Test-Path -LiteralPath $ngoHostFile) -and (Test-Path -LiteralPath $ngoClientFile))) {
        if ([DateTime]::UtcNow -gt $ngoDeadline) { throw "검증 시간 초과: $ngoResultPath" }
        Start-Sleep -Milliseconds 250
    }
    $ngoHostReport = Get-Content -LiteralPath $ngoHostFile -Raw | ConvertFrom-Json
    $ngoClientReport = Get-Content -LiteralPath $ngoClientFile -Raw | ConvertFrom-Json
    if (-not ($ngoHostReport.passed -and $ngoClientReport.passed)) {
        throw "피어 검증 실패. JSON과 로그를 확인하세요: $ngoResultPath"
    }
    foreach ($ngoPlayer in $ngoHostReport.players) {
        $ngoReplica = @($ngoClientReport.players | Where-Object objectId -eq $ngoPlayer.objectId)
        if ($ngoReplica.Count -ne 1 -or $ngoReplica[0].ownerId -ne $ngoPlayer.ownerId) {
            throw '양쪽 피어의 오브젝트 ID 또는 소유자가 일치하지 않습니다.'
        }
        foreach ($ngoAxis in @('x', 'y', 'z')) {
            if ([Math]::Abs($ngoPlayer.position.$ngoAxis - $ngoReplica[0].position.$ngoAxis) -gt 0.05) {
                throw "위치 동기화 검증 실패: object=$($ngoPlayer.objectId), axis=$ngoAxis"
            }
        }
    }
    Write-Output "PASS: 접속, 소유권, 이동 동기화, 벽 충돌 / 결과: $ngoResultPath"
} finally {
    # 이 스크립트가 직접 실행한 프로세스만 정리합니다. 기존 Unity/NGO 창은 건드리지 않습니다.
    foreach ($ngoProcess in $ngoProcesses) {
        if (-not $ngoProcess.HasExited) {
            $null = $ngoProcess.CloseMainWindow()
            if (-not $ngoProcess.WaitForExit(2000)) { $ngoProcess.Kill() }
        }
    }
}
