# BDO-Font-Patcher

Simple, clean, and easy to use font patcher for **Black Desert Online**. Replace the in-game font with any TTF font of your choice.

## Features

- 🎨 Select any `.ttf` font file from your system
- 📁 Auto-detects the BDO game folder (looks for `BlackDesert` in path)
- ⚡ One-click install: copies the selected font to `prestringtable\font\pearl.ttf`
- 🔊 Sound effects on all interactions
- 🌙 Dark theme UI

## Prerequisites

| Requirement | Version |
|-------------|---------|
| **OS** | Windows 10 or later |
| **.NET SDK** | .NET 8.0 SDK ([download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)) |
| **Build tool** | `dotnet` CLI or Visual Studio 2022 |

## Build & Run

### Quick build (CLI)

```bash
# Build
dotnet build -c Release

# Run
dotnet run -c Release
```

### Single-file publish (standalone EXE)

Use this only when you want to distribute the app as a single `.exe`:

```bash
dotnet publish -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true

# Output: bin\Release\net8.0-windows\win-x64\publish\Universal Font Patcher BDO.exe
```

## Creating a GitHub Release

The repository includes a GitHub Actions workflow that automatically builds and publishes releases. To create a release:

```bash
# Tag with a version number and push
git tag v1.0.0
git push origin v1.0.0
```

This triggers the workflow to:
1. Build the project on a Windows runner
2. Generate the single-file EXE
3. Create a GitHub Release with the EXE attached

Update `RELEASE_NOTES.md` before tagging to customize the release notes.

The EXE is fully self-contained — no external DLLs or .NET runtime required.

## Usage

1. Launch the application
2. Click **Select game folder** and navigate to your BDO installation directory
3. Click **Select font** and choose a `.ttf` font file
4. Click **Use selected font** to apply it
5. Restart BDO to see the new font

## Project Structure

```
BDO-Font-Patcher/
├── Universal Font Patcher BDO.sln
├── Universal Font Patcher BDO/
│   ├── Universal Font Patcher BDO.csproj    ← SDK-style, zero deps
│   ├── Program.cs                            ← Entry point
│   ├── Patcher.cs                            ← Main form logic
│   └── Patcher.Designer.cs                   ← UI layout
└── openspec/                                 ← SDD artifacts
```

## Tech Stack

- **Language**: C# (.NET 8.0)
- **UI**: Windows Forms (native, zero third-party controls)
- **Packaging**: `dotnet publish --single-file` (native)
- **Dependencies**: None (0 NuGet packages)

## Chaining PRs

This project was migrated from .NET Framework 4.7.2 using a chaining-PR strategy:

| PR | Branch | Change |
|----|--------|--------|
| #1 | `pr/1-sdk-style` | SDK-style `.csproj` + PackageReference |
| #2 | `pr/2-native-ui` | Replace Guna/Siticone with native WinForms |
| #3 | `pr/3-net8` | Retarget to .NET 8.0 + single-file publish |
| #4 | `pr/4-modernize` | Code modernization (file-scoped namespaces, nullable) |

## License

Original work by [@Sehyn](https://github.com/Sehyn).
