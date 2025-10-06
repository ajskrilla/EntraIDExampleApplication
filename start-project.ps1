# Define paths to the backend and frontend
#$backendPath = "C:\code\EntraIDScannerWebServer\csharp-back\EntraIDScanner.API"
#$frontendPath = "C:\code\EntraIDScannerWebServer\node-server"
#$mongoPath = "C:\Program Files\MongoDB\Server\8.0\bin\mongod.exe"
#$npmPath = "C:\Program Files\nodejs\npm.cmd"

<#
.SYNOPSIS
  Starts MongoDB, the C# backend, and the Node.js frontend.

.PARAMETER MongoExePath
  Full path to the mongod.exe.

.PARAMETER MongoDbPath
  Path to the data directory for MongoDB (passed as --dbpath).

.PARAMETER BackendProjectPath
  The path to your .NET project folder (where you’d run `dotnet run --project …`).

.PARAMETER FrontendPath
  The path to your Node.js project (where you’d run `npm start`).

.PARAMETER DotNetExePath
  (Optional) Full path to the dotnet executable, or just “dotnet” if on PATH.

.PARAMETER NpmExePath
  (Optional) Full path to the npm.cmd executable, or just “npm” if on PATH.
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$MongoExePath,

    [Parameter(Mandatory=$true)]
    [string]$MongoDbPath,

    [Parameter(Mandatory=$true)]
    [string]$BackendProjectPath,

    [Parameter(Mandatory=$true)]
    [string]$FrontendPath,

    [Parameter(Mandatory=$false)]
    [string]$DotNetExePath = "dotnet",

    [Parameter(Mandatory=$false)]
    [string]$NpmExePath    = "npm"
)

function Log {
    param([string]$Message)
    $t = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    Write-Host "[$t] $Message"
}

try {
    Log "Starting MongoDB..."
    Start-Process -NoNewWindow -FilePath $MongoExePath -ArgumentList "--dbpath", $MongoDbPath
    Log "MongoDB launch command issued."

    Log "Starting C# Backend API..."
    Start-Process -NoNewWindow -FilePath $DotNetExePath -ArgumentList "run","--project",$BackendProjectPath
    Log ".NET backend launch command issued."

    Log "Starting Node.js Frontend..."
    Start-Process -NoNewWindow -WorkingDirectory $FrontendPath -FilePath $NpmExePath -ArgumentList "start"
    Log "Node.js frontend launch command issued."

    Log "All services started successfully!"
}
catch {
    Log "ERROR: $($_.Exception.Message)"
    exit 1
}

<#
.\Start-Project.ps1 `
  -MongoExePath       "C:\Program Files\MongoDB\Server\8.0\bin\mongod.exe" `
  -MongoDbPath        "C:\data\db" `
  -BackendProjectPath "C:\code\EntraIDScannerWebServer\csharp-back\EntraIDScanner.API" `
  -FrontendPath       "C:\code\EntraIDScannerWebServer\node-server" `
  -DotNetExePath      "dotnet" `
  -NpmExePath         "C:\Program Files\nodejs\npm.cmd"
#>