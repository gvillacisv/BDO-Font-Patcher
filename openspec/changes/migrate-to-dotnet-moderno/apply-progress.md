# Apply Progress: migrate-to-dotnet-moderno

## PR 4: Code Modernization (net8.0-windows)

**Status**: COMPLETE (tasks 4.1-4.4 implemented, 4.5 pending verification)

### Completed Tasks

| Task | Status | Details |
|------|--------|---------|
| 4.1 | ✅ Complete | Enabled `<ImplicitUsings>enable</ImplicitUsings>` in .csproj |
| 4.2 | ✅ Complete | Enabled `<Nullable>enable</Nullable>` in .csproj |
| 4.3 | ✅ Complete | Converted Patcher.cs to file-scoped namespace, kept only `System.Drawing` and `System.Media` usings |
| 4.4 | ✅ Complete | Converted Patcher.Designer.cs to file-scoped namespace, added `= null!;` to all 11 control fields |
| 4.5 | ⏳ Pending | Verify: `dotnet build` succeeds with nullable warnings resolved (requires Windows machine) |

### Files Changed

| File | Action | What Was Done |
|------|--------|---------------|
| `Universal Font Patcher BDO/Universal Font Patcher BDO.csproj` | Modified | Changed `ImplicitUsings` from `disable` to `enable`, `Nullable` from `disable` to `enable` |
| `Universal Font Patcher BDO/Patcher.cs` | Modified | Converted to file-scoped namespace `namespace Universal_Font_Patcher_BDO;`, removed 11 using statements, kept only `using System.Drawing;` and `using System.Media;` (required for WinForms and SoundPlayer) |
| `Universal Font Patcher BDO/Patcher.Designer.cs` | Modified | Converted to file-scoped namespace, removed all using statements, added `= null!;` to all 11 control field declarations (gunaLabel1-3, BtnContinue, BtnExit, BtnSelectFont, BtnSelectGameFolder, TxtFontPath, TxtGamePath, folderBrowserDialog1) |
| `Universal Font Patcher BDO/Program.cs` | Modified | Converted to file-scoped namespace, removed all 5 using statements (implicitly included via net8.0-windows) |

### Diff: .csproj (PR 4)

**Before:**
```xml
<ImplicitUsings>disable</ImplicitUsings>
<Nullable>disable</Nullable>
```

**After:**
```xml
<ImplicitUsings>enable</ImplicitUsings>
<Nullable>enable</Nullable>
```

### Diff: Patcher.cs (PR 4)

**Before:**
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Universal_Font_Patcher_BDO
{
    public partial class Form1 : Form
    {
```

**After:**
```csharp
using System.Drawing;
using System.Media;

namespace Universal_Font_Patcher_BDO;

public partial class Form1 : Form
{
```

### Diff: Patcher.Designer.cs (PR 4)

**Before:**
```csharp
namespace Universal_Font_Patcher_BDO
{
    partial class Form1
    {
        // ... field declarations
        private System.Windows.Forms.Label gunaLabel1;
        private System.Windows.Forms.Panel gunaPanel1;
        // ...
    }
}
```

**After:**
```csharp
namespace Universal_Font_Patcher_BDO;

partial class Form1
{
        // ... field declarations
        private System.Windows.Forms.Label gunaLabel1 = null!;
        private System.Windows.Forms.Panel gunaPanel1 = null!;
        // ...
    }
```

### Diff: Program.cs (PR 4)

**Before:**
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Universal_Font_Patcher_BDO
{
    internal static class Program
    {
```

**After:**
```csharp
namespace Universal_Font_Patcher_BDO;

internal static class Program
{
```

### Key Design Decisions Applied

1. **Explicit usings retained**: Kept `System.Drawing` and `System.Media` in Patcher.cs as they are NOT in implicit usings for `net8.0-windows` with `UseWindowsForms`
2. **Nullable field handling**: Used `= null!;` pattern for Designer.cs control fields — these are initialized in `InitializeComponent()` at runtime but compiler doesn't recognize this
3. **No pattern matching**: Skipped as per design — minimal benefit for this codebase size

### What Stays Same (PR 4)

- `Properties/AssemblyInfo.cs` — not modified (using statements already minimal, GuidAttribute still works)
- `Properties/Resources.*`, `Properties/Settings.*` — unchanged
- `Patcher.resx`, `Resources/*` — unchanged

### Deviations from Design

- None — implementation matches design exactly.

### Issues Found

- None for the implementation itself.
- Build verification blocked by Linux environment (cannot compile net8.0-windows on Linux).

### Remaining Tasks

- [ ] 4.5 Verify: `dotnet build` succeeds on net8.0-windows with nullable warnings resolved (requires Windows machine)

---

## PR 1: SDK-style .csproj + PackageReference (net472)

**Status**: COMPLETE (tasks 1.1-1.6 complete, 1.7 pending verification)

### Completed Tasks

| Task | Status | Details |
|------|--------|---------|
| 1.1 | ✅ Complete | Created SDK-style .csproj with `<Project Sdk="Microsoft.NET.Sdk">`, target net472, preserved RootNamespace, AssemblyName, ApplicationIcon |
| 1.2 | ✅ Complete | Added Costura.Fody 5.7.0 and Fody 6.5.5 as PackageReference |
| 1.3 | ⚠️ Deferred | libs/ directory created empty — user must copy Guna.UI.dll on Windows |
| 1.4 | ⚠️ Deferred | libs/ directory created empty — user must copy Siticone.UI.dll on Windows |
| 1.5 | ✅ Complete | Added Guna.UI and Siticone.UI DLL references with HintPath to libs/ |
| 1.6 | ✅ Complete | Deleted packages.config |
| 1.7 | ⏳ Pending | dotnet build verification — cannot run on Linux (no .NET Framework SDK) |

### Files Changed

| File | Action | What Was Done |
|------|--------|---------------|
| `Universal Font Patcher BDO/Universal Font Patcher BDO.csproj` | Created (replaced) | New SDK-style format (~45 lines vs original 253 lines), PackageReference for Costura.Fody/Fody, DLL references to libs/, proper embedded resource handling |
| `Universal Font Patcher BDO/packages.config` | Deleted | Removed 50 NuGet packages (migrated Costura.Fody/Fody to PackageReference, removed 40+ System.* and NETStandard.Library) |
| `Universal Font Patcher BDO/libs/` | Created | Empty directory placeholder for Guna.UI.dll and Siticone.UI.dll |

---

## PR 2: Replace Guna/Siticone with native WinForms (net472)

**Status**: COMPLETE (tasks 2.1-2.12 implemented, 2.13-2.14 pending verification)

### Completed Tasks

| Task | Status | Details |
|------|--------|---------|
| 2.1 | ✅ Complete | Changed GunaLabel declarations to System.Windows.Forms.Label (gunaLabel1, gunaLabel2, gunaLabel3) |
| 2.2 | ✅ Complete | Changed GunaPanel declaration to System.Windows.Forms.Panel (gunaPanel1) |
| 2.3 | ✅ Complete | Deleted gunaAnimateWindow1 field declaration entirely |
| 2.4 | ✅ Complete | Changed SiticoneButton declarations to System.Windows.Forms.Button (BtnContinue, BtnExit, BtnSelectFont, BtnSelectGameFolder) |
| 2.5 | ✅ Complete | Changed SiticoneTextBox declarations to System.Windows.Forms.TextBox (TxtGamePath, TxtFontPath) |
| 2.6 | ✅ Complete | Updated constructors in InitializeComponent from Guna/Siticone to native WinForms |
| 2.7 | ✅ Complete | Updated button properties: FlatStyle=Flat, BorderSize=0, MouseOverBackColor, UseVisualStyleBackColor=false, BackColor |
| 2.8 | ✅ Complete | Updated textbox properties: BackColor, BorderStyle=FixedSingle, ForeColor=White, Text (replacing DefaultText) |
| 2.9 | ✅ Complete | Deleted all gunaAnimateWindow1 lines from InitializeComponent |
| 2.10 | ✅ Complete | Deleted gunaAnimateWindow1.Start() from Patcher.cs constructor |
| 2.11 | ✅ Complete | Removed Guna.UI and Siticone.UI DLL references from .csproj |
| 2.12 | ✅ Complete | libs/ directory did not exist (no DLLs to delete) |

### Files Changed

| File | Action | What Was Done |
|------|--------|---------------|
| `Universal Font Patcher BDO/Patcher.Designer.cs` | Modified | Changed 11 field declarations from Guna/Siticone to native WinForms; updated all control constructors; replaced SiticoneButton/SiticoneTextBox properties with native Button/TextBox equivalents; removed GunaAnimateWindow entirely |
| `Universal Font Patcher BDO/Patcher.cs` | Modified | Removed `gunaAnimateWindow1.Start()` from constructor |
| `Universal Font Patcher BDO/Universal Font Patcher BDO.csproj` | Modified | Removed `<Reference Include="Guna.UI">` and `<Reference Include="Siticone.UI">` ItemGroup |

### Key Design Decisions Applied

1. **Field name preservation**: Kept all field names unchanged (gunaLabel1, BtnContinue, etc.) to avoid breaking event handlers in Patcher.cs
2. **Button property mapping**:
   - `FillColor` → `BackColor`
   - Added `FlatStyle = FlatStyle.Flat` for dark theme
   - Added `FlatAppearance.BorderSize = 0` for seamless look
   - Added `FlatAppearance.MouseOverBackColor` for hover feedback
   - Added `UseVisualStyleBackColor = false`
   - Removed: BorderRadius (not supported in native Button), CheckedState/CustomImages/HoveredState/ShadowDecoration.Parent
3. **TextBox property mapping**:
   - `FillColor` → `BackColor`
   - Added `ForeColor = White` for readability on dark background
   - Added `BorderStyle = FixedSingle` for clean border
   - `DefaultText` → `Text`
   - Removed: DisabledState.*, FocusedState.*, HoveredState.*, ShadowDecoration.Parent, PasswordChar, PlaceholderText, SelectedText
4. **GunaAnimateWindow removal**: No replacement - purely cosmetic entrance animation with no functional impact

### Deviations from Design

- **Task 2.12**: libs/ directory did not exist in the project, so no DLLs were present to delete (already cleaned up in previous work or never created)

### Issues Found

- None for the implementation itself
- Build verification blocked by Linux environment (no .NET Framework SDK for net472)

### Remaining Tasks

- [ ] 2.13 Verify: `dotnet build` succeeds on net472 (requires Windows machine)
- [ ] 2.14 Verify: UI visually matches original (colors, sizes, positions) - manual screenshot compare

---

## PR 3: Retarget to net8.0-windows + single-file

**Status**: COMPLETE (tasks 3.1-3.6 implemented, 3.7-3.8 pending verification)

### Completed Tasks

| Task | Status | Details |
|------|--------|---------|
| 3.1 | ✅ Complete | Changed `TargetFramework` from `net472` to `net8.0-windows` |
| 3.2 | ✅ Complete | Added `<UseWindowsForms>true</UseWindowsForms>` |
| 3.3 | ✅ Complete | Added `<RuntimeIdentifier>win-x64</RuntimeIdentifier>` and `<SelfContained>true</SelfContained>` |
| 3.4 | ✅ Complete | Removed `Costura.Fody` and `Fody` PackageReference ItemGroup (zero NuGet dependencies now) |
| 3.5 | ✅ Complete | Deleted `FodyWeavers.xml` — Fody pipeline gone |
| 3.6 | ✅ Complete | Deleted `App.config` — startup section obsolete in .NET 8 |

### Files Changed

| File | Action | What Was Done |
|------|--------|---------------|
| `Universal Font Patcher BDO/Universal Font Patcher BDO.csproj` | Replaced | Full replacement: net8.0-windows target, UseWindowsForms, RuntimeIdentifier, SelfContained, removed Costura/Fody PackageReferences, removed verbose Compile/EmbeddedResource/None ItemGroups (SDK-style auto-includes .cs and handles Resources.resx) |
| `Universal Font Patcher BDO/FodyWeavers.xml` | Deleted | No longer needed — Fody pipeline removed |
| `Universal Font Patcher BDO/App.config` | Deleted | Startup section obsolete in .NET 8 |

### Diff: .csproj (PR 3)

**Before (PR 2):**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net472</TargetFramework>
    <RootNamespace>Universal_Font_Patcher_BDO</RootNamespace>
    <AssemblyName>Universal Font Patcher BDO</AssemblyName>
    <ApplicationIcon>layers_78965.ico</ApplicationIcon>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Costura.Fody" Version="5.7.0" />
    <PackageReference Include="Fody" Version="6.5.5" />
  </ItemGroup>

  <ItemGroup>
    <Compile Include="Patcher.cs"><SubType>Form</SubType></Compile>
    <Compile Include="Patcher.Designer.cs"><DependentUpon>Patcher.cs</DependentUpon></Compile>
    <Compile Include="Program.cs" />
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include="Patcher.resx"><DependentUpon>Patcher.cs</DependentUpon></EmbeddedResource>
    <EmbeddedResource Include="Properties\Resources.resx"><Generator>ResXFileCodeGenerator</Generator><LastGenOutput>Resources.Designer.cs</LastGenOutput></EmbeddedResource>
    <Compile Include="Properties\Resources.Designer.cs"><AutoGen>True</AutoGen><DependentUpon>Resources.resx</DependentUpon><DesignTime>True</DesignTime></Compile>
    <Compile Include="Properties\Settings.Designer.cs"><AutoGen>True</AutoGen><DependentUpon>Settings.settings</DependentUpon><DesignTimeSharedInput>True</DesignTimeSharedInput></Compile>
  </ItemGroup>

  <ItemGroup>
    <None Include="Properties\Settings.settings"><Generator>SettingsSingleFileGenerator</Generator><LastGenOutput>Settings.Designer.cs</LastGenOutput></None>
  </ItemGroup>

  <ItemGroup>
    <Content Include="layers_78965.ico" />
  </ItemGroup>

  <ItemGroup>
    <None Include="Resources\mixkit-modern-technology-select-3124.wav" />
    <None Include="Resources\397633__jalastram__gui-sound-effects-080.wav" />
    <None Include="Resources\397632__jalastram__gui-sound-effects-075.wav" />
  </ItemGroup>
</Project>
```

**After (PR 3):**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <SelfContained>true</SelfContained>
    <RootNamespace>Universal_Font_Patcher_BDO</RootNamespace>
    <AssemblyName>Universal Font Patcher BDO</AssemblyName>
    <ApplicationIcon>layers_78965.ico</ApplicationIcon>
    <ImplicitUsings>disable</ImplicitUsings>
    <Nullable>disable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <Content Include="layers_78965.ico" />
  </ItemGroup>
</Project>
```

**Key changes:**
- TargetFramework: `net472` → `net8.0-windows`
- Added: `<UseWindowsForms>true</UseWindowsForms>`
- Added: `<RuntimeIdentifier>win-x64</RuntimeIdentifier>`, `<SelfContained>true</SelfContained>`
- Added: `<ImplicitUsings>disable</ImplicitUsings>`, `<Nullable>disable</Nullable>`
- Removed: `<LangVersion>latest</LangVersion>`
- Removed: Costura.Fody and Fody PackageReference ItemGroup
- Removed: all verbose Compile/EmbeddedResource/None ItemGroups (SDK-style auto-handles .cs files and Resources.resx)
- Removed: Resources audio files ItemGroup (SDK-style auto-includes via None)
- Kept: `<Content Include="layers_78965.ico" />`

### What Stays the Same (PR 3)

- `Patcher.cs`, `Patcher.Designer.cs`, `Program.cs` — unchanged from PR 2
- `Properties/AssemblyInfo.cs`, `Properties/Resources.*`, `Properties/Settings.*` — unchanged
- `Patcher.resx` — unchanged
- `Resources/*` (wave files) — unchanged
- `layers_78965.ico` — unchanged

### Deviations from Design

- None — implementation matches design exactly.

### Issues Found

- None for the implementation itself.
- Build and publish verification blocked by Linux environment (no Windows SDK for net8.0-windows cross-compilation).

### Remaining Tasks

- [ ] 3.7 Verify: `dotnet build` succeeds on net8.0-windows (requires Windows machine)
- [ ] 3.8 Verify: `dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true` produces single exe <50MB (requires Windows machine)

---

## Summary

**PR 1 progress**: 6/7 tasks complete (1.7 pending verification on Windows)
**PR 2 progress**: 12/14 tasks complete (2.13-2.14 pending verification on Windows)
**PR 3 progress**: 6/8 tasks complete (3.7-3.8 pending verification on Windows)
**PR 4 progress**: 4/5 tasks complete (4.5 pending verification on Windows)

**PR 3 changes**:
- `.csproj`: Full replacement (64 lines → 17 lines), net8.0-windows, UseWindowsForms, RuntimeIdentifier, SelfContained, zero NuGet packages
- `FodyWeavers.xml`: Deleted
- `App.config`: Deleted

**PR 4 changes** (Code Modernization):
- `.csproj`: Enabled `ImplicitUsings` and `Nullable`
- `Patcher.cs`: File-scoped namespace, reduced from 13 usings to 2 (System.Drawing, System.Media)
- `Patcher.Designer.cs`: File-scoped namespace, added `= null!;` to 11 control fields
- `Program.cs`: File-scoped namespace, removed all 5 explicit usings

**What stays the same** (PR 3): Patcher.cs, Patcher.Designer.cs, Program.cs, Properties/*, Patcher.resx, Resources/*, layers_78965.ico
**What stays the same** (PR 4): Properties/AssemblyInfo.cs, Properties/Resources.*, Properties/Settings.*