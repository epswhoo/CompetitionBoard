#Requires -RunAsAdministrator
<#
    Veröffentlicht die CompetitionBoard-WebApp und installiert/aktualisiert sie als Windows-Dienst.
    Der Dienst hat den Starttyp "Manuell" (startet nicht mit Windows) und wird am Ende dieses Skripts gestartet.
    Die WebApp lauscht auf allen Netzwerkadressen (Bedienung: Port 5080, Anzeige: Port 5081),
    beide Ports werden in der Windows-Firewall für alle Netzwerkprofile freigegeben.

    Aufruf (als Administrator):
        .\install-service.ps1
        .\install-service.ps1 -InstallDir "D:\CompetitionBoard"

    Später starten/stoppen:
        Start-Service CompetitionBoard
        Stop-Service CompetitionBoard
#>
param(
    [string]$InstallDir = "$env:ProgramFiles\CompetitionBoard",
    [string]$ServiceName = "CompetitionBoard",
    [int]$Port = 5080,
    [int]$DisplayPort = 5081
)

$ErrorActionPreference = "Stop"

$project = Join-Path $PSScriptRoot "WebApp\WebApp.csproj"
$publishDir = Join-Path $env:TEMP "CompetitionBoard_publish"

Write-Host "Veröffentliche $project ..."
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
dotnet publish $project -c Release -o $publishDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish fehlgeschlagen." }

$service = Get-Service $ServiceName -ErrorAction SilentlyContinue
if ($service -and $service.Status -ne "Stopped") {
    Write-Host "Stoppe Dienst $ServiceName ..."
    Stop-Service $ServiceName -Force
    $service.WaitForStatus("Stopped", [TimeSpan]::FromSeconds(30))
}

# Die Passwörter aus der installierten appsettings.json übernehmen.
$settingsFile = Join-Path $InstallDir "appsettings.json"
$password = ""
$adminPassword = ""
if (Test-Path $settingsFile) {
    $oldSettings = Get-Content $settingsFile -Raw -Encoding UTF8 | ConvertFrom-Json
    if ($oldSettings.UI.Password) { $password = $oldSettings.UI.Password }
    if ($oldSettings.UI.AdminPassword) { $adminPassword = $oldSettings.UI.AdminPassword }
}

Write-Host "Kopiere nach $InstallDir ..."
New-Item -ItemType Directory -Force $InstallDir | Out-Null
Copy-Item "$publishDir\*" $InstallDir -Recurse -Force

if (-not $password) {
    $password = Read-Host "Passwort für die Bedienung (Port $Port) festlegen, leer = kein Passwort"
}
if (-not $adminPassword) {
    $adminPassword = Read-Host "Admin-Passwort festlegen (darf das Passwort der Bedienung ändern), leer = kein Admin"
}

# Endpunkte auf allen Netzwerkadressen und Passwörter in die installierte appsettings.json schreiben.
$settings = Get-Content $settingsFile -Raw -Encoding UTF8 | ConvertFrom-Json
$settings.Kestrel.Endpoints.Http.Url = "http://0.0.0.0:$Port"
$settings.Kestrel.Endpoints.Display.Url = "http://0.0.0.0:$DisplayPort"
$settings.UI | Add-Member -NotePropertyName DisplayPort -NotePropertyValue $DisplayPort -Force
$settings.UI | Add-Member -NotePropertyName Password -NotePropertyValue $password -Force
$settings.UI | Add-Member -NotePropertyName AdminPassword -NotePropertyValue $adminPassword -Force
$settings | ConvertTo-Json -Depth 10 | Set-Content $settingsFile -Encoding UTF8

$exe = Join-Path $InstallDir "WebApp.exe"
if (-not $service) {
    Write-Host "Registriere Dienst $ServiceName ..."
    New-Service -Name $ServiceName -BinaryPathName "`"$exe`"" -DisplayName "CompetitionBoard" `
        -Description "CompetitionBoard Web-App" -StartupType Manual | Out-Null
    # Bei Absturz nach 10 Sekunden neu starten.
    sc.exe failure $ServiceName reset= 86400 actions= restart/10000/restart/10000/restart/10000 | Out-Null
}
else {
    # Bestehenden Dienst auf den aktuellen Pfad und Starttyp "Manuell" setzen.
    sc.exe config $ServiceName binPath= "`"$exe`"" | Out-Null
    Set-Service $ServiceName -StartupType Manual
}

function Set-FirewallRule([string]$Name, [string]$DisplayName, [int]$LocalPort) {
    $rule = Get-NetFirewallRule -Name $Name -ErrorAction SilentlyContinue
    if ($rule) {
        Set-NetFirewallRule -Name $Name -NewDisplayName $DisplayName -Direction Inbound -Protocol TCP `
            -LocalPort $LocalPort -Action Allow -Profile Any -RemoteAddress Any -Enabled True
    }
    else {
        New-NetFirewallRule -Name $Name -DisplayName $DisplayName -Direction Inbound -Protocol TCP `
            -LocalPort $LocalPort -Action Allow -Profile Any -RemoteAddress Any | Out-Null
    }
}

Write-Host "Gebe Ports $Port und $DisplayPort in der Firewall frei ..."
Set-FirewallRule $ServiceName "CompetitionBoard (TCP $Port)" $Port
Set-FirewallRule "$ServiceName-Display" "CompetitionBoard Anzeige (TCP $DisplayPort)" $DisplayPort

Write-Host "Starte Dienst $ServiceName ..."
Start-Service $ServiceName
Get-Service $ServiceName | Format-Table Name, Status, StartType -AutoSize

$ips = Get-NetIPAddress -AddressFamily IPv4 |
    Where-Object { $_.IPAddress -ne "127.0.0.1" -and $_.PrefixOrigin -ne "WellKnown" } |
    Select-Object -ExpandProperty IPAddress
Write-Host "Erreichbar unter:"
foreach ($ip in $ips) {
    Write-Host "  Bedienung: http://${ip}:$Port   Anzeige: http://${ip}:$DisplayPort"
}
