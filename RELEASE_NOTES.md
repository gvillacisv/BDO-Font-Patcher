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
