param([string]$Configuration="Release")
$ErrorActionPreference="Stop"
$root=Split-Path $PSScriptRoot -Parent
Set-Location $root
dotnet restore
dotnet build PokemonROI/PokemonROI.csproj -c $Configuration
dotnet test PokemonROI.Tests/PokemonROI.Tests.csproj -c $Configuration --no-restore
$release=Join-Path $root "Release"
if(Test-Path $release){Remove-Item $release -Recurse -Force}
dotnet publish PokemonROI/PokemonROI.csproj -c $Configuration -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o $release
Write-Host "PokemonROI.exe: $release\PokemonROI.exe"
