$ErrorActionPreference = 'Stop'

& dotnet msbuild "$PSScriptRoot/Directory.Build.props" -getProperty:Tool_TestProject |
    Tee-Object -Variable testProject

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to get BenchmarkProject property."
    exit 1
}

if (-not (Test-Path $testProject -PathType Leaf)) {
    Write-Error "File not exists: $testProject"
    exit 1
}

$testProject = [System.IO.Path]::GetFullPath($testProject)
& dotnet test --project "$testProject"  2>&1

