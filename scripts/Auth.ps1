<#
.SYNOPSIS
  Posts Azure credentials to your auth endpoint.

.PARAMETER Uri
  The full URL of the auth API. Defaults to http://localhost:3000/api/auth.

.PARAMETER TenantId
  Your Azure tenant ID.

.PARAMETER ClientId
  Your Azure app (client) ID.

.PARAMETER ClientSecret
  Your Azure app client secret.

.EXAMPLE
  .\Send-Creds.ps1 -TenantId contoso -ClientId abc123 -ClientSecret supersecret

  Uses the default URI, logs each step, and prints the response.
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$Uri = "http://localhost:3000/api/auth",

    [Parameter(Mandatory=$true)]
    [string]$TenantId,

    [Parameter(Mandatory=$true)]
    [string]$ClientId,

    [Parameter(Mandatory=$true)]
    [string]$ClientSecret
)

function Log {
    param([string]$Message)
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    Write-Host "[$timestamp] $Message"
}

try {
    Log "Preparing to send credentials to $Uri"
    $headers = @{ "Content-Type" = "application/json" }
    $body = @{
        tenantId     = $TenantId
        clientId     = $ClientId
        clientSecret = $ClientSecret
    } | ConvertTo-Json -Depth 10

    Log "Request body:"
    Write-Host $body

    Log "Sending POST request..."
    $response = Invoke-RestMethod -Uri $Uri -Method Post -Headers $headers -Body $body -ErrorAction Stop

    Log "Received response:"
    Write-Output $response
}
catch {
    Log "ERROR: $($_.Exception.Message)"
    exit 1
}
<#
.\Auth.ps1 `
  -Uri 'https://localhost:3000/api/auth' `
  -TenantId 'your-tenant-id' `
  -ClientId 'your-client-id' `
  -ClientSecret 'your-client-secret'
#>