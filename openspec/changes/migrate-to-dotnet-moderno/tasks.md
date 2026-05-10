# Tasks: migrate-to-dotnet-moderno

## PR 1: SDK-style .csproj + PackageReference (net472)

- [x] 1.1 Create SDK-style `.csproj` in `Universal Font Patcher BDO/` — convert legacy format to `Project Sdk="Microsoft.NET.Sdk"`, keep `net472`, preserve output/assembly settings
- [x] 1.2 Add `Costura.Fody` and `Fody` as PackageReference in `.csproj` (version 5.7.0 and 6.5.5)
- [x] 1.3 Copy `Guna.UI.dll` from Desktop to `libs/Guna.UI.dll` — copy from developer's Guna.UI.dll location
- [x] 1.4 Copy `Siticone.UI.dll` from Desktop to `libs/Siticone.UI.dll` — copy from developer's Siticone.UI.dll location
- [x] 1.5 Add DLL references in `.csproj` to `libs/Guna.UI.dll` and `libs/Siticone.UI.dll`
- [x] 1.6 Delete `packages.config` — no longer needed with PackageReference
- [ ] 1.7 Verify: `dotnet build` succeeds on net472, no build errors

## PR 2: Replace Guna/Siticone with native WinForms (net472)

- [x] 2.1 Update `Patcher.Designer.cs` — change `GunaLabel` declarations to `System.Windows.Forms.Label` (gunaLabel1, gunaLabel2, gunaLabel3)
- [x] 2.2 Update `Patcher.Designer.cs` — change `GunaPanel` declaration to `System.Windows.Forms.Panel` (gunaPanel1)
- [x] 2.3 Update `Patcher.Designer.cs` — delete `gunaAnimateWindow1` field declaration entirely
- [x] 2.4 Update `Patcher.Designer.cs` — change `SiticoneButton` declarations to `System.Windows.Forms.Button` (BtnContinue, BtnExit, BtnSelectFont, BtnSelectGameFolder)
- [x] 2.5 Update `Patcher.Designer.cs` — change `SiticoneTextBox` declarations to `System.Windows.Forms.TextBox` (TxtGamePath, TxtFontPath)
- [x] 2.6 Update `Patcher.Designer.cs` InitializeComponent — update constructors from `new Guna.UI.WinForms.*()` to `new System.Windows.Forms.*()`
- [x] 2.7 Update `Patcher.Designer.cs` InitializeComponent — update button properties: set `FlatStyle=FlatStyle.Flat`, `FlatAppearance.BorderSize=0`, `FlatAppearance.MouseOverBackColor`, `UseVisualStyleBackColor=false`, `BackColor=dark color`
- [x] 2.8 Update `Patcher.Designer.cs` InitializeComponent — update textbox properties: set `BackColor=dark`, `BorderStyle=FixedSingle`, `ForeColor=White`
- [x] 2.9 Update `Patcher.Designer.cs` InitializeComponent — delete all `gunaAnimateWindow1` lines (constructor, properties)
- [x] 2.10 Update `Patcher.cs` — delete `gunaAnimateWindow1.Start()` line from constructor
- [x] 2.11 Update `.csproj` — remove `Guna.UI` and `Siticone.UI` DLL references from ItemGroup
- [x] 2.12 Delete `libs/Guna.UI.dll` and `libs/Siticone.UI.dll` (libs/ directory does not exist - no DLLs to delete)
- [ ] 2.13 Verify: `dotnet build` succeeds on net472
- [ ] 2.14 Verify: UI visually matches original (colors, sizes, positions)

## PR 3: Retarget to net8.0-windows + single-file

- [ ] 3.1 Update `.csproj` — change `TargetFramework` from `net472` to `net8.0-windows`
- [ ] 3.2 Update `.csproj` — add `<UseWindowsForms>true</UseWindowsForms>`
- [ ] 3.3 Update `.csproj` — add `<RuntimeIdentifier>win-x64</RuntimeIdentifier>` and `<SelfContained>true</SelfContained>`
- [ ] 3.4 Update `.csproj` — remove `Costura.Fody` and `Fody` PackageReference lines
- [ ] 3.5 Delete `FodyWeavers.xml` — no longer needed
- [ ] 3.6 Delete `App.config` — startup section obsolete in .NET 8
- [ ] 3.7 Verify: `dotnet build` succeeds on net8.0-windows
- [ ] 3.8 Verify: `dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true` produces single exe <50MB

## PR 4: Code Modernization (Optional)

- [ ] 4.1 Update `.csproj` — change `<ImplicitUsings>disable</ImplicitUsings>` to `enable`
- [ ] 4.2 Update `.csproj` — change `<Nullable>disable</Nullable>` to `enable`
- [ ] 4.3 Update `Patcher.cs` — convert to file-scoped namespace, remove redundant usings, keep `using System.Drawing;` and `using System.Media;`
- [ ] 4.4 Update `Patcher.Designer.cs` — convert to file-scoped namespace
- [ ] 4.5 Update `Program.cs` — convert to file-scoped namespace, remove redundant usings
- [ ] 4.6 Update `Properties/AssemblyInfo.cs` — add `#nullable disable` or null-forgiving to avoid warnings
- [ ] 4.7 Verify: `dotnet build` succeeds with zero warnings

---

## Implementation Order

PRs must execute in order: 1 → 2 → 3 → 4.

Within each PR, tasks flow from file creation/modification to verification.

- **PR 1**: Foundation — new build system
- **PR 2**: UI layer — replace controls, preserve functionality
- **PR 3**: Platform — retarget and publish
- **PR 4**: Polish — modernize code (optional)

## Verification Checklist

| PR | Build | Visual | Publish |
|----|-------|--------|---------|
| 1 | dotnet build net472 | N/A | N/A |
| 2 | dotnet build net472 | Manual screenshot compare | N/A |
| 3 | dotnet build net8.0 | Compare to PR2 | dotnet publish single-file |
| 4 | dotnet build zero warnings | Same as PR3 | Same as PR3 |