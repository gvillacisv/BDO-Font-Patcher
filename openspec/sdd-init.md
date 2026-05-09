# SDD Init — BDO-Font-Patcher

> Generado: 2026-05-09
> Persistencia: `openspec` (file-based)

## Tech Stack

| Capa | Tecnología |
|------|-----------|
| **Lenguaje** | C# (.NET Framework 4.7.2) |
| **UI Framework** | Windows Forms (WinExe) |
| **Build System** | MSBuild via Visual Studio 2022 (Solution Format v12) |
| **UI Libraries** | Guna.UI (third-party), Siticone.UI (third-party) |
| **Packaging** | Costura.Fody (embeds dependencies into single EXE) |
| **Runtime** | .NET Framework 4.7.2 (App.config) |

## Architecture

- **Pattern**: Single-form event-driven Windows Forms application (code-behind, no MVVM/MVC)
- **Entry**: `Program.cs` → `Application.Run(new Form1())`
- **Main Form**: `Patcher.cs` + `Patcher.Designer.cs` (Form1)
- **UI Theme**: Dark-themed custom UI (gunmetal grays: #28-#40 range)
- **Interactions**: Sound effects on all button clicks/checkbox changes (.wav resources embebidos)
- **Functionality**: Selects a TTF font file, copies it to BDO game directory's `prestringtable/font/` folder as `pearl.ttf`

## Project Structure

```
BDO-Font-Patcher/
├── Universal Font Patcher BDO.sln
├── README.md
├── .atl/
│   └── skill-registry.md
├── openspec/                        ← SDD artifact store
│   ├── index.md
│   ├── sdd-init.md
│   ├── testing-capabilities.md
│   ├── skill-registry.md
│   └── explorations/
│       └── build-dependencies.md
└── Universal Font Patcher BDO/
    ├── Universal Font Patcher BDO.csproj
    ├── Program.cs                    ← Entry point
    ├── Patcher.cs                    ← Main form logic (Form1)
    ├── Patcher.Designer.cs           ← Designer-generated UI layout
    ├── App.config                    ← .NET runtime config
    ├── packages.config               ← NuGet packages
    ├── FodyWeavers.xml               ← Fody weaver config (Costura)
    ├── FodyWeavers.xsd
    ├── layers_78965.ico              ← Application icon
    ├── Properties/
    │   ├── AssemblyInfo.cs
    │   ├── Resources.resx / Resources.Designer.cs
    │   └── Settings.settings / Settings.Designer.cs
    └── Resources/
        ├── mixkit-modern-technology-select-3124.wav
        ├── 397633__jalastram__gui-sound-effects-080.wav
        └── 397632__jalastram__gui-sound-effects-075.wav
```

## NuGet Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Costura.Fody | 5.7.0 | Assembly embedding into single EXE |
| Fody | 6.5.5 | IL weaving infrastructure |
| NETStandard.Library | 1.6.1 | .NET Standard compatibility bridge |
| Guna.UI | — (local DLL) | Third-party WinForms controls |
| Siticone.UI | — (local DLL) | Third-party WinForms controls |
| System.\* (multiple) | 4.3.0 | .NET Standard API compatibility |

## Conventions

- No AGENTS.md, CLAUDE.md, .cursorrules, or GEMINI.md found
- No project-level skills directory
- Git repository initialized (single branch: `main`)
- Assembly version: 1.0.0.0
- Copyright: © 2021

## Strict TDD Mode

**Disabled** — No test project or test runner detected.

## Session Info

- **Session**: manual-save-bdo-font-patcher
- **Project**: bdo-font-patcher
- **Scope**: project
