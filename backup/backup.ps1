<#
.SYNOPSIS
  Snapshot-style backup of a project, excluding specified folders.

.PARAMETER ProjectDir
  The root of your project. Defaults to the current directory.

.NOTES
  Requires Robocopy (built into modern Windows).
  Save as backup.ps1 and run with:
    PS> .\backup.ps1
  or
    PS> .\backup.ps1 -ProjectDir "C:\path\to\my\project"
#>

param(
    [string]$ProjectDir = (Get-Location).Path
)

# ——— CONFIG ———
# Top-level backup root
$BackupRoot = Join-Path $ProjectDir 'backup'
# Folders to skip entirely
$Excludes    = @('node_modules',' .git','backup')
# —————————

# Timestamp for this snapshot
$Timestamp = Get-Date -Format 'yyyy-MM-dd_HHmmss'
$DestDir   = Join-Path $BackupRoot $Timestamp

# 1) Ensure the backup root exists
if (-not (Test-Path $BackupRoot)) {
    Write-Host "Creating backup root: $BackupRoot"
    New-Item -Path $BackupRoot -ItemType Directory | Out-Null
}

# 2) Create the timestamped snapshot folder
Write-Host "Creating snapshot folder: $DestDir"
New-Item -Path $DestDir -ItemType Directory | Out-Null

# 3) Build full paths for Robocopy's /XD (exclude directories) switch
$excludePaths = $Excludes | ForEach-Object { Join-Path $ProjectDir $_ }

# 4) Assemble Robocopy arguments
$robocopyArgs = @(
    $ProjectDir,       # Source
    $DestDir,          # Destination
    '/E',              # Copy all subfolders, including empty ones
    '/Z',              # Restartable mode
    '/R:3',            # Retry 3 times on failure
    '/W:5',            # Wait 5 seconds between retries
    '/NFL','/NDL',     # No file/dir logging (cleaner output)
    '/XD'              # Begin exclude-dirs list
) + $excludePaths

# 5) Run Robocopy
Write-Host "Starting backup from `"$ProjectDir`" to `"$DestDir`" (excluding: $($Excludes -join ', '))"
& robocopy @robocopyArgs | ForEach-Object { Write-Host $_ }

Write-Host "`n✅ Backup complete ➜ $DestDir"
