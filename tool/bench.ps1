$ErrorActionPreference = 'Stop'

$benchmarkProject = & dotnet msbuild "$PSScriptRoot/Directory.Build.props" -getProperty:BenchmarkProject

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to get BenchmarkProject property."
    exit 1
}

if (-not (Test-Path $benchmarkProject -PathType Leaf)) {
    Write-Error "File not exists: $benchmarkProject"
    exit 1
}

Push-Location (Join-Path $benchmarkProject "..")

$benchmarkProject = [System.IO.Path]::GetFullPath($benchmarkProject)
& dotnet run --project "$benchmarkProject" -c Release -f net10.0  2>&1

Pop-Location
