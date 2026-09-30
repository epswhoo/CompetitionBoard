#Requires -RunAsAdministrator
<#
    Stoppt und entfernt den CompetitionBoard-Windows-Dienst. Die veröffentlichten Dateien bleiben erhalten.
#>
param(
    [string]$ServiceName = "CompetitionBoard"
)

$ErrorActionPreference = "Stop"

$service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if (-not $service) {
    Write-Host "Dienst $ServiceName ist nicht installiert."
    return
}

if ($service.Status -ne "Stopped") {
    Stop-Service -Name $ServiceName
    $service.WaitForStatus("Stopped", [TimeSpan]::FromSeconds(30))
}
sc.exe delete $ServiceName | Out-Null
Write-Host "Dienst $ServiceName wurde entfernt."
