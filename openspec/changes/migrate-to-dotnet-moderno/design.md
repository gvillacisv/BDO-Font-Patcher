# Design: migrate-to-dotnet-moderno

## Current Architecture

Single-form WinForms app targeting .NET Framework 4.7.2. Windows-only (WinForms). Three layers:

- **Build system**: Legacy `.csproj` (250 lines, 50 NuGet packages via `packages.config`, Costura.Fody for assembly embedding, Fody weaver pipeline)
- **UI layer**: Third-party controls — Guna.UI (labels, panel, animation window) + Siticone.UI (buttons, text boxes). Referenced as raw DLLs from developer Desktop path.
- **Business logic**: `Patcher.cs` (form code-behind) — font file copy, folder validation, sound effects. No data layer. No tests.

## Target Architecture

Single-form WinForms app targeting **net8.0-windows**, SDK-style `.csproj` (~15 lines), zero NuGet packages, single-file publish, fully native WinForms controls.

```
┌──────────────────────────────────────────┐
│           SDK-style .csproj              │
│  <TargetFramework>net8.0-windows</...>   │
│  <UseWindowsForms>true</...>             │
│  <PublishSingleFile>true</...>           │
├──────────────────────────────────────────┤
│         Native WinForms Controls         │
│  Label · Panel · Button · TextBox        │
│  (no Guna, no Siticone)                  │
├──────────────────────────────────────────┤
│         Business Logic (unchanged)       │
│  File.Copy · FolderBrowserDialog         │
│  SoundPlayer · MessageBox                │
└──────────────────────────────────────────┘
```

## PR 1 — SDK-style + PackageReference (net472)

### Current .csproj → SDK-style .csproj

**Before** (legacy, ~250 lines):
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="..\packages\Costura.Fody.5.7.0\build\Costura.Fody.props" />
  <PropertyGroup>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <OutputType>WinExe</OutputType>
    ...
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="Costura, ..."><HintPath>..\packages\...\Costura.dll</HintPath></Reference>
    <Reference Include="Guna.UI"><HintPath>..\..\..\..\Desktop\Dev\Guna.UI.dll</HintPath></Reference>
    <Reference Include="Siticone.UI"><HintPath>..\..\..\..\Desktop\Dev\Siticone.UI.dll</HintPath></Reference>
    <Reference Include="System" />
    <Reference Include="System.AppContext, ..."><HintPath>..\packages\...\System.AppContext.dll</HintPath></Reference>
    <!-- 40+ more System.* and NETStandard.Library shims -->
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Drawing" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Patcher.cs"><SubType>Form</SubType></Compile>
    ...
    <EmbeddedResource Include="Properties\Resources.resx" .../>
    <None Include="packages.config" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <Import Project="..\packages\Fody.6.5.5\build\Fody.targets" />
  <!-- NuGet package restore validation targets -->
</Project>
```

**After** (SDK-style, ~25 lines):
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net472</TargetFramework>
    <RootNamespace>Universal_Font_Patcher_BDO</RootNamespace>
    <AssemblyName>Universal Font Patcher BDO</AssemblyName>
    <ApplicationIcon>layers_78965.ico</ApplicationIcon>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Costura.Fody" Version="5.7.0" />
    <PackageReference Include="Fody" Version="6.5.5" />
  </ItemGroup>
  <ItemGroup>
    <Reference Include="Guna.UI">
      <HintPath>libs\Guna.UI.dll</HintPath>
    </Reference>
    <Reference Include="Siticone.UI">
      <HintPath>libs\Siticone.UI.dll</HintPath>
    </Reference>
  </ItemGroup>
  <ItemGroup>
    <Content Include="layers_78965.ico" />
  </ItemGroup>
</Project>
```

### PackageReference Mapping (from packages.config)

| packages.config Entry | Action | Rationale |
|---|---|---|
| `Costura.Fody` 5.7.0 | Keep as PackageReference | Still needed for assembly embedding (removed in PR 3) |
| `Fody` 6.5.5 | Keep as PackageReference | Weaver infrastructure (removed in PR 3) |
| `NETStandard.Library` 1.6.1 | **Remove** | SDK-style auto-references framework assemblies |
| `Microsoft.NETCore.Platforms` 1.1.0 | **Remove** | Transitive of NETStandard.Library |
| `Microsoft.Win32.Primitives` 4.3.0 | **Remove** | In-box in .NET Framework, SDK auto-refs |
| `System.*` (40+ packages: System.IO, System.Runtime, System.Linq, etc.) | **Remove all** | SDK-style auto-references framework assemblies for net472 |
| `Guna.UI` (DLL ref) | Keep as DLL ref, relocate to `libs/` | Not a NuGet package; needed until PR 2 |
| `Siticone.UI` (DLL ref) | Keep as DLL ref, relocate to `libs/` | Not a NuGet package; needed until PR 2 |

### What Stays the Same

- `Patcher.cs` — **unchanged** (no code modifications)
- `Patcher.Designer.cs` — **unchanged** (control declarations untouched)
- `Program.cs` — **unchanged**
- `App.config` — **unchanged** (still needed for net472 runtime selection)
- `Patcher.resx` — **unchanged**
- `Properties/Resources.resx`, `Resources.Designer.cs` — **unchanged**
- `Properties/Settings.settings`, `Settings.Designer.cs` — **unchanged**
- `FodyWeavers.xml` — **unchanged** (Costura still active)
- `.gitignore` — **unchanged**

### What Changes

- `Universal Font Patcher BDO.csproj` — converted to SDK-style
- `packages.config` — **deleted** (migrated to PackageReference)
- Guna.UI.dll and Siticone.UI.dll — **copied** from Desktop path to project-local `libs/` folder

### Risks

| Risk | Mitigation |
|---|---|
| SDK-style handles `SubType` differently | Ensure `Patcher.cs` has `<SubType>Form</SubType>` — SDK-style may auto-detect partial Form classes |
| Embedded resource paths may differ | SDK-style uses `EmbeddedResource` with `Generator` and `LastGenOutput` for Resources.resx |

---

## PR 2 — Native Controls Replacement

### Control Mapping Table

| Old Control | New Control | Key Property Changes |
|---|---|---|
| `GunaLabel gunaLabel1` | `Label gunaLabel1` | No functional change; all core properties identical |
| `GunaLabel gunaLabel2` | `Label gunaLabel2` | Same; zero-size label (serves as spacer) |
| `GunaLabel gunaLabel3` | `Label gunaLabel3` | Same |
| `GunaPanel gunaPanel1` | `Panel gunaPanel1` | Same backcolor, dock, child controls |
| `GunaAnimateWindow gunaAnimateWindow1` | **REMOVED** | No replacement; purely cosmetic entrance animation |
| `SiticoneButton BtnExit` | `Button BtnExit` | FlatStyle + FlatAppearance for dark theme |
| `SiticoneButton BtnContinue` | `Button BtnContinue` | FlatStyle + FlatAppearance for dark theme |
| `SiticoneButton BtnSelectGameFolder` | `Button BtnSelectGameFolder` | FlatStyle + FlatAppearance for dark theme |
| `SiticoneButton BtnSelectFont` | `Button BtnSelectFont` | FlatStyle + FlatAppearance for dark theme |
| `SiticoneTextBox TxtGamePath` | `TextBox TxtGamePath` | BackColor, ForeColor, BorderStyle, ReadOnly |
| `SiticoneTextBox TxtFontPath` | `TextBox TxtFontPath` | BackColor, ForeColor, BorderStyle, ReadOnly |

### Declaration Changes (field declarations at bottom of Designer.cs)

**Before:**
```csharp
private Guna.UI.WinForms.GunaLabel gunaLabel1;
private Guna.UI.WinForms.GunaPanel gunaPanel1;
private Guna.UI.WinForms.GunaLabel gunaLabel2;
private Siticone.UI.WinForms.SiticoneButton BtnContinue;
private Siticone.UI.WinForms.SiticoneButton BtnExit;
private Guna.UI.WinForms.GunaAnimateWindow gunaAnimateWindow1;
private Siticone.UI.WinForms.SiticoneButton BtnSelectFont;
private Siticone.UI.WinForms.SiticoneButton BtnSelectGameFolder;
private Siticone.UI.WinForms.SiticoneTextBox TxtFontPath;
private Siticone.UI.WinForms.SiticoneTextBox TxtGamePath;
private Guna.UI.WinForms.GunaLabel gunaLabel3;
private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
```

**After:**
```csharp
private System.Windows.Forms.Label gunaLabel1;
private System.Windows.Forms.Panel gunaPanel1;
private System.Windows.Forms.Label gunaLabel2;
private System.Windows.Forms.Button BtnContinue;
private System.Windows.Forms.Button BtnExit;
// gunaAnimateWindow1 REMOVED
private System.Windows.Forms.Button BtnSelectFont;
private System.Windows.Forms.Button BtnSelectGameFolder;
private System.Windows.Forms.TextBox TxtFontPath;
private System.Windows.Forms.TextBox TxtGamePath;
private System.Windows.Forms.Label gunaLabel3;
private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
```

### InitializeComponent Changes — Exact Mappings

#### GunaLabel gunaLabel1 → Label gunaLabel1
```
// OLD (replaced):
this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
//
// gunaLabel1
//
this.gunaLabel1.AutoSize = true;
this.gunaLabel1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
this.gunaLabel1.ForeColor = System.Drawing.Color.White;
this.gunaLabel1.Location = new System.Drawing.Point(10, 11);
this.gunaLabel1.Name = "gunaLabel1";
this.gunaLabel1.Size = new System.Drawing.Size(163, 14);
this.gunaLabel1.TabIndex = 0;
this.gunaLabel1.Text = "Universal Font Patcher [BDO]";

// NEW:
this.gunaLabel1 = new System.Windows.Forms.Label();

this.gunaLabel1.AutoSize = true;
this.gunaLabel1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
this.gunaLabel1.ForeColor = System.Drawing.Color.White;
this.gunaLabel1.Location = new System.Drawing.Point(10, 11);
this.gunaLabel1.Name = "gunaLabel1";
this.gunaLabel1.Size = new System.Drawing.Size(163, 14);
this.gunaLabel1.TabIndex = 0;
this.gunaLabel1.Text = "Universal Font Patcher [BDO]";
```
**Change**: Only the constructor type changes (`Guna.UI.WinForms.GunaLabel` → `System.Windows.Forms.Label`). All property lines identical.

#### GunaPanel gunaPanel1 → Panel gunaPanel1
```
// OLD:
this.gunaPanel1 = new Guna.UI.WinForms.GunaPanel();
// NEW:
this.gunaPanel1 = new System.Windows.Forms.Panel();
```
All properties identical: `BackColor`, `Controls.Add`, `Dock`, `Location`, `Name`, `Size`, `TabIndex`.

#### GunaAnimateWindow gunaAnimateWindow1 → **REMOVED**
Delete these 5 lines entirely:
```csharp
// OLD - DELETE:
this.gunaAnimateWindow1 = new Guna.UI.WinForms.GunaAnimateWindow(this.components);
// 
// gunaAnimateWindow1
// 
this.gunaAnimateWindow1.AnimationType = Guna.UI.WinForms.GunaAnimateWindow.AnimateWindowType.AW_CENTER;
this.gunaAnimateWindow1.Interval = 100;
this.gunaAnimateWindow1.TargetControl = this;
```

Also delete `gunaAnimateWindow1.Start();` from `Patcher.cs` line 26:
```csharp
// OLD - DELETE from Form1() constructor:
gunaAnimateWindow1.Start();
```

#### SiticoneButton BtnExit → Button BtnExit
```
// OLD:
this.BtnExit = new Siticone.UI.WinForms.SiticoneButton();
//
// BtnExit
//
this.BtnExit.CheckedState.Parent = this.BtnExit;
this.BtnExit.CustomImages.Parent = this.BtnExit;
this.BtnExit.FillColor = System.Drawing.Color.FromArgb(40, 40, 40);
this.BtnExit.Font = new System.Drawing.Font("Segoe UI", 9F);
this.BtnExit.ForeColor = System.Drawing.Color.White;
this.BtnExit.HoveredState.Parent = this.BtnExit;
this.BtnExit.Location = new System.Drawing.Point(421, 7);
this.BtnExit.Name = "BtnExit";
this.BtnExit.ShadowDecoration.Parent = this.BtnExit;
this.BtnExit.Size = new System.Drawing.Size(51, 24);
this.BtnExit.TabIndex = 26;
this.BtnExit.Text = "X";
this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);

// NEW:
this.BtnExit = new System.Windows.Forms.Button();
this.BtnExit.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
this.BtnExit.FlatAppearance.BorderSize = 0;
this.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(60, 60, 60);
this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
this.BtnExit.Font = new System.Drawing.Font("Segoe UI", 9F);
this.BtnExit.ForeColor = System.Drawing.Color.White;
this.BtnExit.Location = new System.Drawing.Point(421, 7);
this.BtnExit.Name = "BtnExit";
this.BtnExit.Size = new System.Drawing.Size(51, 24);
this.BtnExit.TabIndex = 26;
this.BtnExit.Text = "X";
this.BtnExit.UseVisualStyleBackColor = false;
this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
```

**Key changes**:
- `FillColor` → `BackColor`
- Removed: `CheckedState.Parent`, `CustomImages.Parent`, `HoveredState.Parent`, `ShadowDecoration.Parent`
- Added: `FlatStyle = FlatStyle.Flat`, `FlatAppearance.BorderSize = 0`, `FlatAppearance.MouseOverBackColor`, `UseVisualStyleBackColor = false`
- No `BorderRadius` equivalent in native Button (corner radius not supported without custom painting)

#### SiticoneButton BtnContinue, BtnSelectGameFolder, BtnSelectFont → Button (same pattern)
Same transformation as BtnExit, with these per-button specifics:

```csharp
// Each button:
this.BtnContinue = new System.Windows.Forms.Button();
this.BtnContinue.BackColor = System.Drawing.Color.FromArgb(35, 35, 35);
this.BtnContinue.FlatAppearance.BorderSize = 0;
this.BtnContinue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(55, 55, 55);
this.BtnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
this.BtnContinue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
this.BtnContinue.ForeColor = System.Drawing.Color.White;
this.BtnContinue.Location = new System.Drawing.Point(13, 128);  // varies per button
this.BtnContinue.Name = "BtnContinue";  // varies
this.BtnContinue.Size = new System.Drawing.Size(459, 30);  // varies
this.BtnContinue.TabIndex = 25;  // varies
this.BtnContinue.Text = "Use selected font";  // varies
this.BtnContinue.UseVisualStyleBackColor = false;
this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
```

#### SiticoneTextBox TxtGamePath / TxtFontPath → TextBox
```
// OLD:
this.TxtGamePath = new Siticone.UI.WinForms.SiticoneTextBox();
//
// TxtGamePath
//
this.TxtGamePath.Cursor = System.Windows.Forms.Cursors.IBeam;
this.TxtGamePath.DefaultText = "C:\\..";
this.TxtGamePath.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
this.TxtGamePath.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
this.TxtGamePath.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
this.TxtGamePath.DisabledState.Parent = this.TxtGamePath;
this.TxtGamePath.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
this.TxtGamePath.FillColor = System.Drawing.Color.FromArgb(35, 35, 35);
this.TxtGamePath.FocusedState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
this.TxtGamePath.FocusedState.Parent = this.TxtGamePath;
this.TxtGamePath.HoveredState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
this.TxtGamePath.HoveredState.Parent = this.TxtGamePath;
this.TxtGamePath.Location = new System.Drawing.Point(195, 44);
this.TxtGamePath.Name = "TxtGamePath";
this.TxtGamePath.PasswordChar = '\0';
this.TxtGamePath.PlaceholderText = "";
this.TxtGamePath.ReadOnly = true;
this.TxtGamePath.SelectedText = "";
this.TxtGamePath.ShadowDecoration.Parent = this.TxtGamePath;
this.TxtGamePath.Size = new System.Drawing.Size(277, 36);
this.TxtGamePath.TabIndex = 28;

// NEW:
this.TxtGamePath = new System.Windows.Forms.TextBox();
this.TxtGamePath.BackColor = System.Drawing.Color.FromArgb(35, 35, 35);
this.TxtGamePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
this.TxtGamePath.Cursor = System.Windows.Forms.Cursors.IBeam;
this.TxtGamePath.ForeColor = System.Drawing.Color.White;
this.TxtGamePath.Location = new System.Drawing.Point(195, 44);
this.TxtGamePath.Name = "TxtGamePath";
this.TxtGamePath.ReadOnly = true;
this.TxtGamePath.Size = new System.Drawing.Size(277, 36);
this.TxtGamePath.TabIndex = 28;
this.TxtGamePath.Text = "C:\\..";
this.TxtGamePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
```

**Key changes**:
- `FillColor` → `BackColor` (with dark value #232323)
- Added `ForeColor = White` for readability on dark background
- `DefaultText` → `Text` property
- `PasswordChar` → removed (default `\0` is standard)
- `PlaceholderText = ""` → removed (empty is default)
- `SelectedText = ""` → removed (not needed at initialization)
- Removed: all `DisabledState.*`, `FocusedState.*`, `HoveredState.*`, `ShadowDecoration.Parent`
- Added: `BorderStyle = FixedSingle` (gives clean border like Siticone)
- Note: SiticoneTextBox had a visual height of 36px; native TextBox may size differently. The explicit `Size` preserves layout.

### Patcher.cs Changes

**Only change**: Remove `gunaAnimateWindow1.Start();` from constructor (line 26 in current file):
```csharp
public Form1()
{
    InitializeComponent();
    // gunaAnimateWindow1.Start(); ← DELETE THIS LINE
    SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound);
    audio.Play();
}
```

All event handlers (`BtnContinue_Click`, `BtnExit_Click`, `BtnSelectGameFolder_Click`, `BtnSelectFont_Click`) remain **completely unchanged** — they reference the same field names (`TxtGamePath`, `TxtFontPath`).

### .csproj Changes for PR 2

Remove Guna.UI and Siticone.UI DLL references from the .csproj:
```xml
<!-- DELETE these from <ItemGroup>: -->
<Reference Include="Guna.UI">
  <HintPath>libs\Guna.UI.dll</HintPath>
</Reference>
<Reference Include="Siticone.UI">
  <HintPath>libs\Siticone.UI.dll</HintPath>
</Reference>
```

### What Stays the Same (PR 2)

- `App.config` unchanged
- `FodyWeavers.xml` unchanged
- `packages.config` already deleted in PR 1
- Target framework still `net472`
- Costura.Fody and Fody PackageReferences unchanged (still needed for Costura embedding)
- Resource files unchanged

---

## PR 3 — net8.0-windows + Single-file

### Final .csproj (exact expected output)

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

**Total**: 18 lines. **Zero** NuGet package references.

### Package Removal Plan

| Package | Removed In | Why |
|---|---|---|
| `Costura.Fody` 5.7.0 | PR 3 | Replaced by `dotnet publish --single-file` |
| `Fody` 6.5.5 | PR 3 | No longer needed without Costura |
| `Guna.UI` (DLL) | PR 2 | Replaced by native WinForms |
| `Siticone.UI` (DLL) | PR 2 | Replaced by native WinForms |
| `NETStandard.Library` | PR 1 | SDK-style handles framework refs |
| All 40+ `System.*` packages | PR 1 | SDK-style handles framework refs |

### FodyWeavers.xml → Deleted

File removed entirely. No replacement needed — Fody pipeline is gone.

### App.config Change

For net8.0-windows, the `<startup>` section is obsolete. App.config can be:
- **Deleted** entirely (no settings are read from it in the app), OR
- Simplified to just the config section (if settings need it). Given the app only has the startup section, delete it.

### dotnet publish Command

```bash
dotnet publish -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:DebugType=embedded
```

Expected output: `bin/Release/net8.0-windows/win-x64/publish/Universal Font Patcher BDO.exe` — single file, <50MB.

### What Changes (PR 3)

| File | Action |
|---|---|
| `.csproj` | TargetFramework → `net8.0-windows`, add `UseWindowsForms`, add `RuntimeIdentifier`, add `SelfContained`, remove Costura/Fody PackageReferences |
| `App.config` | Delete or clear obsolete startup section |
| `FodyWeavers.xml` | Delete file |

### What Stays the Same

- `Patcher.cs` — **unchanged** from PR 2
- `Patcher.Designer.cs` — **unchanged** from PR 2
- `Program.cs` — **unchanged** (Application.EnableVisualStyles/SetCompatibleTextRenderingDefault still work in .NET 8)
- `Patcher.resx` — unchanged
- `Properties/Resources.*`, `Properties/Settings.*` — unchanged
- `layers_78965.ico` — unchanged

### Migration Notes

- `Application.EnableVisualStyles()` and `Application.SetCompatibleTextRenderingDefault(false)` are still valid in .NET 8 WinForms
- `System.Media.SoundPlayer` is in-box in .NET 8 (part of `System.Windows.Extensions` which is brought in by `UseWindowsForms`)
- `FolderBrowserDialog`, `OpenFileDialog`, `MessageBox` — all in-box
- `System.IO.Directory`, `File`, `Path` — in-box in `System.Runtime`
- If Resources.Designer.cs was generated with a specific `StronglyTypedResourceBuilder` version, it may need regeneration under .NET 8 SDK. The auto-generated code uses `global::System.Resources.ResourceManager` which is still in-box.

---

## PR 4 (Optional) — Code Modernization

### File-scoped Namespaces

**Patcher.cs before:**
```csharp
namespace Universal_Font_Patcher_BDO
{
    public partial class Form1 : Form
    {
        // ...
    }
}
```

**After:**
```csharp
namespace Universal_Font_Patcher_BDO;

public partial class Form1 : Form
{
    // ...
}
```

**Program.cs before:**
```csharp
namespace Universal_Font_Patcher_BDO
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
```

**After:**
```csharp
namespace Universal_Font_Patcher_BDO;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }
}
```

**Patcher.Designer.cs before:**
```csharp
namespace Universal_Font_Patcher_BDO
{
    partial class Form1
    {
        // ...
    }
}
```

**After:**
```csharp
namespace Universal_Font_Patcher_BDO;

partial class Form1
{
    // ...
}
```

### Implicit Usings

**In .csproj**, change:
```xml
<ImplicitUsings>disable</ImplicitUsings>
```
to:
```xml
<ImplicitUsings>enable</ImplicitUsings>
```

Then remove explicit `using` lines from .cs files that are covered by implicit usings. For `net8.0-windows` with `UseWindowsForms`, the implicit usings include: `System`, `System.Collections.Generic`, `System.IO`, `System.Linq`, `System.Net.Http`, `System.Threading`, `System.Threading.Tasks`, `System.Windows.Forms`, and more.

**Patcher.cs — remove**:
```csharp
using System;
using System.Collections.Generic;
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
```

**Patcher.Designer.cs — remove** (if any `using` lines exist at the top).

**Program.cs — remove**:
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
```

Note: `System.Media` is NOT part of the implicit usings in `net8.0-windows` with `UseWindowsForms`. The `SoundPlayer` references in `Patcher.cs` use `System.Media.SoundPlayer` fully qualified, so they will still compile. Alternatively, add `using System.Media;` if preferred.

`System.Drawing` is NOT implicitly included either. The Designer.cs references `System.Drawing.Color`, `System.Drawing.Font`, `System.Drawing.Point`, `System.Drawing.Size` — these are either used with full qualification (`System.Drawing.Color.FromArgb(...)`) or will need `using System.Drawing;` retained.

### Nullable Reference Types

**In .csproj**, change:
```xml
<Nullable>disable</Nullable>
```
to:
```xml
<Nullable>enable</Nullable>
```

**Code review per file**:
- `Program.cs` — no nullable warnings expected (simple static entry point)
- `Patcher.cs` — event handler EventArgs parameters may need `EventArgs?` depending on strictness. Variables like `string sourceFile` etc. are immediately assigned, so no warnings expected.
- `Patcher.Designer.cs` — control fields are initialized in `InitializeComponent()`, but the compiler may warn because they're not assigned in the constructor before the method is called. Options:
  a. Add `= null!;` or `= default!;` to field declarations
  b. Add `#nullable disable` around the designer region
  c. Leave fields as-is with `[AllowNull]` or similar

The safest approach for Designer.cs:
```csharp
#pragma warning disable CS8618  // Non-nullable field is uninitialized
private System.Windows.Forms.Label gunaLabel1 = null!;
private System.Windows.Forms.Panel gunaPanel1 = null!;
// etc.
#pragma warning restore CS8618
```

### Pattern Matching (if desired)

Current code uses:
```csharp
if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
```

Could become (no real benefit in this codebase):
```csharp
if (result is DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
```

Given the small size of this codebase, pattern matching offers minimal improvement. Recommend **skipping** unless explicitly wanted.

### ImplicitUsings Explicit Usings Decision

For net8.0-windows + UseWindowsForms, the implicit usings include:
- `System` ✓
- `System.Collections.Generic` ✓  
- `System.IO` ✓
- `System.Linq` ✓
- `System.Net.Http` ✓ (not used, harmless)
- `System.Threading` ✓
- `System.Threading.Tasks` ✓
- `System.Windows.Forms` ✓

NOT implicitly included (must keep explicit or use fully qualified):
- `System.Drawing` (used in Designer.cs for Color, Font, Point, Size)
- `System.Media` (used for SoundPlayer in Patcher.cs — but currently uses fully qualified `new SoundPlayer(...)` which resolves via `using System.Media;` in the current code)

Best practice: Enable `ImplicitUsings`, keep only `using System.Drawing;` and `using System.Media;` explicitly.

---

## Architecture Decisions

### Decision: Chained PRs over single big-bang migration

| Option | Tradeoff | Decision |
|---|---|---|
| Single PR (all changes at once) | Faster but impossible to bisect issues | ❌ |
| **4 chained PRs** (build, UI, framework, polish) | Each is independently verifiable and reversible; clear blame isolation | ✅ |

### Decision: Keep field names unchanged during control replacement

| Option | Tradeoff | Decision |
|---|---|---|
| Rename `gunaLabel1` → `label1` | Cleaner naming but breaks event handler references in Patcher.cs | ❌ |
| **Keep old field names** | No changes to Patcher.cs event handlers; visual diff is smaller | ✅ |

### Decision: SDK-style with net472 in PR 1, then retarget in PR 3

| Option | Tradeoff | Decision |
|---|---|---|
| Jump directly to net8.0-windows | Risks build failures mixing Guna/Siticone (not compatible with .NET 8) | ❌ |
| **Two-step: net472 SDK first, then retarget** | Identifies SDK-conversion issues separately from framework-retargeting issues | ✅ |

### Decision: Use `dotnet publish --single-file` instead of Costura.Fody

| Option | Tradeoff | Decision |
|---|---|---|
| Keep Costura.Fody | Works today but adds Fody pipeline complexity; doesn't support .NET 8 well | ❌ |
| **dotnet publish --single-file** | Native .NET 8 feature, zero dependencies, actively maintained by Microsoft | ✅ |

### Decision: Remove GunaAnimateWindow with no replacement

| Option | Tradeoff | Decision |
|---|---|---|
| Replace with custom animation | Adds complexity for purely cosmetic effect; form is small so entrance animation barely visible | ❌ |
| **Remove entirely** | Simplest change; no functional impact | ✅ |

---

## File Changes Summary

| File | PR 1 | PR 2 | PR 3 | PR 4 |
|---|---|---|---|---|
| `.csproj` | ✅ SDK-style + PackageRef | ✅ Remove DLL refs | ✅ net8.0-windows, single-file | ✅ ImplicitUsings/Nullable |
| `packages.config` | ✅ Delete | — | — | — |
| `FodyWeavers.xml` | — | — | ✅ Delete | — |
| `App.config` | — | — | ✅ Delete | — |
| `Patcher.Designer.cs` | — | ✅ Guna→Label, Siticone→Btn/Txt | — | ✅ File-scoped ns |
| `Patcher.cs` | — | ✅ Remove gunaAnimateWindow1.Start() | — | ✅ File-scoped ns |
| `Program.cs` | — | — | — | ✅ File-scoped ns |
| `libs/Guna.UI.dll` | ✅ Copy from Desktop | ✅ Delete | — | — |
| `libs/Siticone.UI.dll` | ✅ Copy from Desktop | ✅ Delete | — | — |

## Testing Strategy

| Layer | What | Approach |
|---|---|---|
| Build | Each PR compiles | `dotnet build` on net472 (PR 1-2) then net8.0-windows (PR 3-4) |
| Visual | Pixel-perfect UI | Manual comparison: screenshot original vs. new; verify colors, sizes, positions |
| Functional | Font patching | Manual: select folder + font, click "Use selected font", verify file copy |
| Publish | Single-file | `dotnet publish` per PR 3 command; verify exe runs standalone |
| Sound | Sound effects | Manual: verify click sounds play (checkbox, button, error) |

## Migration/Rollout

PRs must be merged in order 1→2→3→4(optional). Each PR is independently reversible via `git revert`. Branch strategy: `main` → `pr/1-sdk-style` → `pr/2-native-controls` → `pr/3-net8` → `pr/4-modernize`.

No data migration required. Zero external services involved.

## Open Questions

- [ ] Does the `Designer.cs` control naming (`gunaLabel1` etc.) cause confusion for future maintenance? (Decision: keep for minimal diff, accept the cosmetic naming oddity)
- [ ] Will the Resources.Designer.cs auto-generated code need regeneration under .NET 8 SDK? (Likely works as-is since it uses framework types available in .NET 8, but should be verified)
- [ ] Is `System.Media` included in implicit usings for `net8.0-windows`? (No — explicit `using System.Media;` or full qualification needed if `ImplicitUsings` is enabled)
- [ ] Button `BorderRadius` (rounded corners) — Siticone had 3px radius; native Button with `FlatStyle.Flat` does not support rounded corners without owner-draw. Acceptable loss?
