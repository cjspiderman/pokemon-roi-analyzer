# Pokémon ROI Analyzer v2

A Windows WPF desktop application for card research, transparent price data, portfolio tracking, and scenario-based ROI calculations.

## Current implementation
- .NET 8 WPF application, dark finance-style interface
- Live search through the public Pokémon TCG API (no key required)
- Source and timestamp shown for returned market prices
- Local SQLite database under `%LOCALAPPDATA%\PokemonROI`
- Portfolio add/delete, cost basis, current value, profit/loss, ROI, and CSV export
- Decimal-based fee, ROI, CAGR, and compound-growth calculation services
- Offline-safe startup: cached portfolio remains available; provider failures return no invented values
- Self-contained `win-x64` single-file publish script

## Build and run
On a Windows machine with the .NET 8 SDK:
```powershell
.\build-release.ps1
.\Release\PokemonROI.exe
```
For development:
```powershell
.\run-dev.ps1
```

## Data and privacy
The app uses the public Pokémon TCG API endpoint. No API key is embedded. Portfolio data is stored locally. The application does not claim that projections are predictions or investment advice.

## Provider limitations
The Pokémon TCG API supplies card metadata and TCGPlayer market fields when present. PSA/BGS/CGC prices, historical series, eBay sold listings, and additional marketplace adapters are intentionally not fabricated; those integrations require separate licensed/API access and are not represented as available data.

## Release
`build-release.ps1` restores packages, builds, runs tests, and publishes a self-contained executable to `Release\PokemonROI.exe`. The publish output may include supporting native extraction files depending on the installed .NET SDK and SQLite native assets.
