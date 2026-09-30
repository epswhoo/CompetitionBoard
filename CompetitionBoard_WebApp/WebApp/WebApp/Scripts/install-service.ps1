#Requires -RunAsAdministrator
<#
    Veröffentlicht die CompetitionBoard-WebApp und installiert sie als Windows-Dienst.
    Ist der Dienst bereits installiert, wird er gestoppt, aktualisiert und neu gestartet.

    Aufruf (als Administrator):
        .\install-service.ps1
        .\install-service.ps1 -PublishDir "D:\CompetitionBoard"
#>
param(
    [string]$PublishDir = "C:\Services\CompetitionBoard",
    [string]$ServiceName = "CompetitionBoard"
)

$ErrorActionPreference = "Stop"
$project = Join-Path $PSScriptRoot "..\WebApp.csproj"

$service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($service -and $service.Status -ne "Stopped") {
    Write-Host "Stoppe Dienst $ServiceName ..."
    Stop-Service -Name $ServiceName
    $service.WaitForStatus("Stopped", [TimeSpan]::FromSeconds(30))
}

# Eine bereits angepasste appsettings.json im Zielordner nicht überschreiben.
$settingsFile = Join-Path $PublishDir "appsettings.json"
$settingsBackup = $null
if (Test-Path $settingsFile) {
    $settingsBackup = Get-Content $settingsFile -Raw
}

Write-Host "Veröffentliche nach $PublishDir ..."
dotnet publish $project -c Release -o $PublishDir
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish ist fehlgeschlagen."
}

if ($settingsBackup) {
    Set-Content -Path $settingsFile -Value $settingsBackup -Encoding utf8 -NoNewline
    Write-Host "Vorhandene appsettings.json wurde beibehalten."
}

if (-not $service) {
    Write-Host "Installiere Dienst $ServiceName ..."
    New-Service -Name $ServiceName `
        -BinaryPathName "`"$(Join-Path $PublishDir 'WebApp.exe')`"" `
        -DisplayName "CompetitionBoard" `
        -Description "Anzeigetafel für Reitturniere (Blazor-WebApp)" `
        -StartupType Automatic | Out-Null
    # Bei Absturz nach 5 Sekunden neu starten.
    sc.exe failure $ServiceName reset= 86400 actions= restart/5000/restart/5000/restart/5000 | Out-Null
}

Write-Host "Starte Dienst $ServiceName ..."
Start-Service -Name $ServiceName
Get-Service -Name $ServiceName
