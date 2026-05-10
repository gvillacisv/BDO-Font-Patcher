# Delta Specifications: migrate-to-dotnet-moderno

## Goal
Migrate BDO-Font-Patcher from .NET Framework 4.7.2 to .NET 8.0, eliminating all external dependencies. Replace Guna.UI/Siticone.UI with native WinForms, remove Costura.Fody, convert to SDK-style .csproj with PackageReference. Result: zero NuGet packages, ~15-line .csproj.

## Requirements

### Functional Requirements
1. [ ] Convert legacy .csproj to SDK-style format
2. [ ] Migrate packages.config to inline PackageReference
3. [ ] Keep net472 as target for PR 1 (no behavior change)
4. [ ] Replace GunaLabel with System.Windows.Forms.Label
5. [ ] Replace GunaPanel with System.Windows.Forms.Panel
6. [ ] Remove GunaAnimateWindow entirely (effect was just cosmetic entrance animation)
7. [ ] Replace SiticoneButton with System.Windows.Forms.Button with FlatStyle.Flat + dark colors
8. [ ] Replace SiticoneTextBox with System.Windows.Forms.TextBox with dark backcolor
9. [ ] Maintain EXACT visual appearance (same colors, sizes, font, positions)
10. [ ] Update #region Windows Form Designer generated code properly
11. [ ] Change TargetFramework to net8.0-windows
12. [ ] Remove Costura.Fody, Fody, NETStandard.Library references
13. [ ] Remove FodyWeavers.xml
14. [ ] Remove all unnecessary package references (40+ System.* packages)
15. [ ] Enable UseWindowsForms
16. [ ] Add dotnet publish --single-file support
17. [ ] Result: zero NuGet packages, minimal .csproj
18. [ ] (Optional) File-scoped namespaces
19. [ ] (Optional) Implicit usings
20. [ ] (Optional) Nullable reference types
21. [ ] (Optional) Pattern matching where appropriate

### Non-Functional Requirements
1. [ ] Builds on .NET 8.0 SDK
2. [ ] Published executable runs Windows-only without external deps
3. [ ] Single-file size <50MB
4. [ ] UI visually identical to original
5. [ ] Core font-patching functionality identical
6. [ ] Final .csproj <20 lines, zero NuGet refs

## Assumptions
1. The business logic in Patcher.cs remains unchanged except for UI control references
2. Resources (icons, sounds, etc.) remain unchanged
3. App.config and settings files remain unchanged
4. The FolderBrowserDialog and OpenFileDialog are already native System.Windows.Forms controls and require no changes
5. SoundPlayer is already native System.Media and requires no changes
6. The development environment has Windows 10+ and Visual Studio 2022 with .NET 8.0 SDK

## Constraints
1. Must maintain exact visual appearance (colors, sizes, font, positions)
2. Cannot change business logic (Patcher.cs functionality beyond UI control updates)
3. Must preserve all existing functionality including sound effects and folder validation
4. Each PR must be independently reversible
5. Zero breaking changes between PRs

## Open Questions
1. Are there any specific Guna.UI or Siticone.UI properties or events used that don't have direct equivalents in native WinForms?
2. What is the exact color values used for the dark theme? (Need to extract from current implementation)
3. Are there any animation effects beyond the GunaAnimateWindow that need to be considered?
4. What version of .NET 8.0 should be targeted (preview vs stable)?

## Files Modified

### PR 1: SDK-style .csproj + PackageReference (net472)
- Universal Font Patcher BDO/Universal Font Patcher BDO.csproj (converted to SDK-style, PackageReference)
- Universal Font Patcher BDO/packages.config (removed)
- Universal Font Patcher BDO/App.config (unchanged)
- Universal Font Patcher BDO/Patcher.cs (unchanged)
- Universal Font Patcher BDO/Patcher.Designer.cs (unchanged)
- Universal Font Patcher BDO/Patcher.resx (unchanged)
- Universal Font Patcher BDO/FodyWeavers.xml (unchanged)
- Universal Font Patcher BDO/Resources/* (unchanged)
- Universal Font Patcher BDO/Program.cs (unchanged)
- Universal Font Patcher BDO/Properties/* (unchanged)

### PR 2: Replace Guna/Siticone with native WinForms (net472)
- Universal Font Patcher BDO/Patcher.cs (updated control references and event handlers)
- Universal Font Patcher BDO/Patcher.Designer.cs (updated control declarations and properties)
- Universal Font Patcher BDO/Universal Font Patcher BDO.csproj (updated PackageReferences)
- Universal Font Patcher BDO/App.config (unchanged)
- Universal Font Patcher BDO/Patcher.resx (unchanged)
- Universal Font Patcher BDO/FodyWeavers.xml (unchanged)
- Universal Font Patcher BDO/Resources/* (unchanged)
- Universal Font Patcher BDO/Program.cs (unchanged)
- Universal Font Patcher BDO/Properties/* (unchanged)

### PR 3: Retarget to net8.0-windows + single-file
- Universal Font Patcher BDO/Universal Font Patcher BDO.csproj (target framework update, dependency removal)
- Universal Font Patcher BDO/FodyWeavers.xml (removed)
- Universal Font Patcher BDO/Patcher.cs (unchanged from PR 2)
- Universal Font Patcher BDO/Patcher.Designer.cs (unchanged from PR 2)
- Universal Font Patcher BDO/App.config (unchanged)
- Universal Font Patcher BDO/Patcher.resx (unchanged)
- Universal Font Patcher BDO/Resources/* (unchanged)
- Universal Font Patcher BDO/Program.cs (unchanged)
- Universal Font Patcher BDO/Properties/* (unchanged)

### PR 4 (Optional) - Code modernization
- Universal Font Patcher BDO/Patcher.cs (file-scoped namespaces, implicit usings, nullable)
- Universal Font Patcher BDO/Program.cs (file-scoped namespaces, implicit usings, nullable)
- Universal Font Patcher BDO/Properties/*.cs (file-scoped namespaces, implicit usings, nullable)
- Universal Font Patcher BDO/Universal Font Patcher BDO.csproj (implicit usings, nullable enabled)

## Acceptance Criteria

### PR 1 Acceptance Criteria
- [ ] Project builds successfully with SDK-style format
- [ ] PackageReference replaces packages.config
- [ ] Target framework remains net472
- [ ] No changes to .cs files
- [ ] All existing functionality preserved
- [ ] Build output identical to previous (except for build system changes)

### PR 2 Acceptance Criteria
- [ ] All Guna.UI references replaced with System.Windows.Forms.Label
- [ ] All GunaPanel references replaced with System.Windows.Forms.Panel
- [ ] GunaAnimateWindow completely removed
- [ ] All SiticoneButton references replaced with System.Windows.Forms.Button with appropriate styling
- [ ] All SiticoneTextBox references replaced with System.Windows.Forms.TextBox with appropriate styling
- [ ] Visual appearance identical to original (pixel-perfect match)
- [ ] All event handlers preserved and functioning
- [ ] FolderBrowserDialog and OpenFileDialog unchanged (already native)
- [ ] SoundPlayer unchanged (already native)
- [ ] No changes to business logic in event handlers
- [ ] Project still targets net472

### PR 3 Acceptance Criteria
- [ ] Target framework changed to net8.0-windows
- [ ] Costura.Fody, Fody, NETStandard.Library references removed
- [ ] FodyWeavers.xml removed
- [ ] Unnecessary System.* package references removed (keeping only what's needed)
- [ ] UseWindowsForms enabled
- [ ] dotnet publish --single-file works correctly
- [ ] Resulting .csproj is minimal (<20 lines) with zero external NuGet references
- [ ] Published executable runs without external dependencies
- [ ] Single-file size <50MB
- [ ] UI visually identical to original
- [ ] Core font-patching functionality identical

### PR 4 Acceptance Criteria (Optional)
- [ ] File-scoped namespaces implemented where appropriate
- [ ] Implicit usings enabled
- [ ] Nullable reference types enabled
- [ ] Pattern matching used where appropriate
- [ ] No functional changes from PR 3
- [ ] Code compiles with zero warnings

## Risks & Mitigations

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| UI layout changes | Medium | Extract exact property values from current implementation; verify pixel-perfect match |
| Missing native WinForms APIs | Low | Research equivalents; create simple wrappers if needed |
| SDK-style build issues | Low | Follow Microsoft migration guides; test incrementally |
| Single-file complications | Low | Test with dotnet publish --self-contained and --single-file |
| Assembly resolution after removal | Low | Verify .NET 8 contains needed types; keep only necessary references |
| Event handler signature mismatches | Low | Ensure exact method signatures when replacing controls |
| Loss of animation effects | Low | Confirm GunaAnimateWindow was purely cosmetic; no functional dependency |

## Dependencies
- Windows 10+ development environment (unchanged)
- Visual Studio 2022 with .NET 8.0 SDK (required for PR 3)
- Zero external dependencies post-migration

## Rollback Plan
Each PR independently reversible:
- PR 1: Revert .csproj to legacy format, restore packages.config
- PR 2: Restore Guna.UI/Siticone.UI references and DLLs
- PR 3: Retarget to net472, restore Fody/Costura/packages
- Business logic unchanged ensures rollback maintains functionality
- Git enables easy PR revert if post-merge issues arise

## Success Criteria
- [ ] Builds on .NET 8.0 SDK
- [ ] Published executable runs Windows-only without external deps
- [ ] Single-file size <50MB
- [ ] UI visually identical to original
- [ ] Core font-patching functionality identical
- [ ] Final .csproj <20 lines, zero NuGet refs
- [ ] All four chained PRs merge sequentially without breaking changes