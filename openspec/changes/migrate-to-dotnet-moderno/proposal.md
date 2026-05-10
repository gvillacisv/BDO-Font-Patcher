# Proposal: migrate-to-dotnet-moderno

## Intent

Migrate BDO-Font-Patcher from .NET Framework 4.7.2 to .NET 8.0, eliminating all external dependencies. Replace Guna.UI/Siticone.UI with native WinForms, remove Costura.Fody, convert to SDK-style .csproj with PackageReference. Result: zero NuGet packages, ~15-line .csproj.

## Scope

### In Scope
- SDK-style .csproj + PackageReference migration
- Guna.UI/Siticone.UI → native WinForms control replacement
- net472 → net8.0-windows retargeting
- Costura.Fody removal → dotnet publish --single-file
- Fody, NETStandard.Library, 40+ System.* package removal
- Business logic preservation (Patcher.cs unchanged)
- Visual fidelity verification

### Out of Scope
- Business logic changes
- New features
- Non-Windows platform support
- Major refactoring beyond migration
- Automated testing implementation

## Capabilities

### New Capabilities
- `dotnet-8-migration`: .NET 8.0 with single-file publishing
- `native-ui-controls`: Native WinForms only (no third-party deps)
- `simplified-dependency-management`: Zero external NuGet packages

### Modified Capabilities
- `font-patching`: Requirements unchanged; implementation updated for .NET 8

## Approach

Four chained PRs:
1. **PR 1**: SDK-style .csproj + PackageReference (net472)
2. **PR 2**: Replace Guna/Siticone controls with native WinForms (net472)
3. **PR 3**: Retarget to net8.0-windows + single-file; remove Costura/Fody/NETStandard/packages
4. **PR 4** (Optional): Modern C# features (async, nullable, file-scoped namespaces)

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| `.csproj` | Modified | SDK-style conversion, target framework update, dependency removal |
| `packages.config` | Removed | Migrated to PackageReference |
| `FodyWeavers.xml` | Removed | No longer needed |
| `Patcher.cs` | Modified | UI control references updated |
| `Patcher.Designer.cs` | Modified | UI control declarations updated |
| `App.config` | Preserved | No changes |
| `Patcher.resx` | Preserved | No changes |
| `Resources/*` | Preserved | No changes |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| UI layout changes | Medium | Visual verification; match properties/events |
| Missing native WinForms APIs | Low | Research equivalents; create wrappers |
| SDK-style build issues | Low | Follow Microsoft guides; test incrementally |
| Single-file complications | Low | Test with dotnet publish --self-contained |
| Assembly resolution after removal | Low | Verify .NET 8 contains needed types |

## Rollback Plan

Each PR independently reversible:
- PR 1: Revert .csproj to legacy format, restore packages.config
- PR 2: Restore Guna.UI/Siticone.UI references and DLLs
- PR 3: Retarget to net472, restore Fody/Costura/packages
- Business logic unchanged ensures rollback maintains functionality
- Git enables easy PR revert if post-merge issues arise

## Dependencies

- Windows 10+ development environment (unchanged)
- Visual Studio 2022 with .NET 8.0 SDK (required for PR 3)
- Zero external dependencies post-migration

## Success Criteria

- [ ] Builds on .NET 8.0 SDK
- [ ] Published executable runs Windows-only without external deps
- [ ] Single-file size <50MB
- [ ] UI visually identical to original
- [ ] Core font-patching functionality identical
- [ ] Final .csproj <20 lines, zero NuGet refs
- [ ] All four chained PRs merge sequentially without breaking changes