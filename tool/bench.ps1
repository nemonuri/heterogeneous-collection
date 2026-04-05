
[CmdletBinding()]
param (
    [Parameter(Mandatory = $false, ValueFromRemainingArguments)]
    [string[]] $ExtraArgs = @()
)

$ErrorActionPreference = 'Stop'

if ($ExtraArgs.Length -gt 0) {
    Write-Host "ExtraArgs = $($ExtraArgs -join " ")"
}

& dotnet msbuild "$PSScriptRoot/Directory.Build.props" -getProperty:Tool_BenchmarkProject |
    Tee-Object -Variable benchmarkProject

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to get BenchmarkProject property."
    exit 1
}

if (-not (Test-Path $benchmarkProject -PathType Leaf)) {
    Write-Error "File not exists: $benchmarkProject"
    exit 1
}

#--- normalize extra args ---

[string]$normalizedExtraArgs = ""

if ($ExtraArgs.Length -gt 0) {
    $normalizedExtraArgs = "-- $($ExtraArgs -join " ")"
}

#---|

Push-Location (Join-Path $benchmarkProject "..")

$benchmarkProject = [System.IO.Path]::GetFullPath($benchmarkProject)
& dotnet run --project "$benchmarkProject" -c Release -f net10.0 $normalizedExtraArgs  2>&1

Pop-Location
