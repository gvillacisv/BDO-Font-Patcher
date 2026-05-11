# Release v1.1.1

## What's New

Optimized executable size with build trimming and compression.

### Changes

- **Smaller EXE**: Reduced from ~20MB to ~6-8MB using `PublishTrimmed` + `EnableCompressionInSingleFile`
- Removed debug symbols from release build
- Same functionality, smaller download

---

# Release v1.1.0

## What's New

Auto-detection of Black Desert Online game installations with multi-path support, plus polished form redesign and code quality improvements.

### Changes

- **Auto-detect BDO path**: Detects installations from Windows Registry (standalone client), Steam manifests (App ID 836620), and common filesystem paths
- **Multi-installation support**: All detected paths shown as checkboxes — patch one, multiple, or all at once
- **Smart validation**: Each detected path verified with `BlackDesertLauncher.exe` presence
- **Manual override**: Browse button always available to add custom paths
- **Redesigned UI**: Modernized form layout (560x300), updated button colors with blue accent theme (`SystemColors.HotTrack`), improved spacing and alignment
- **Better font validation**: Selected `.ttf` files are validated before patching — no more copying invalid or corrupted fonts
- **Stronger path validation**: Manual folder browsing checks for `BlackDesertLauncher.exe`, not just a folder name match
- **Fixed resource leaks**: All `SoundPlayer` instances properly disposed, font GDI objects reused via static field
- **Code quality**: Zero confirmed issues after adversarial Judgment Day code review
- **Designer compatibility**: Added `nuget.config` and `global.json` for VS 2022 WinForms designer support with .NET 8
- Backward compatible — no changes to existing workflow

---

# Release v1.0.0

## What's New

Modernized port of BDO-Font-Patcher from .NET Framework 4.7.2 to .NET 8.0.

### Changes

- **.NET 8.0**: Migrated from legacy .NET Framework 4.7.2 to modern .NET 8.0
- **Zero dependencies**: Removed all external NuGet packages and third-party UI libraries (Guna.UI, Siticone.UI)
- **Native WinForms**: All UI controls replaced with native Windows Forms equivalents
- **Single-file EXE**: Uses `dotnet publish --single-file` instead of Costura.Fody
- **Cleaner code**: File-scoped namespaces, nullable reference types, implicit usings

### Features

- Select any `.ttf` font file from your system
- Auto-detect Black Desert Online installation folder
- One-click font patching to `prestringtable\font\pearl.ttf`
- Dark theme UI with sound effects
- Standalone EXE — no .NET runtime or DLLs required

### Usage

1. Download `Universal Font Patcher BDO.exe` from Assets below
2. Run on any Windows 10/11 machine (no installation needed)
3. Click **Select game folder** → choose your BDO directory
4. Click **Select font** → choose a `.ttf` file
5. Click **Use selected font**
6. Restart Black Desert Online

### Notes

- Built for `win-x64` — 64-bit Windows only
- Self-contained (~20MB) — includes .NET runtime
- Virus scanners may flag single-file executables — this is a false positive
