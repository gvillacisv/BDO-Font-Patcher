# Change Proposal: Auto-Detect BDO Game Path

> Generated: 2026-05-09
> Persistence: hybrid (engram + openspec)
> Topic key: sdd/auto-detect-game-path/proposal

## Intent

This change solves the problem of manual BDO game path selection by implementing automatic detection of Black Desert Online installations from multiple sources (registry, Steam, common paths). It eliminates the need for users to manually navigate to their game directory while preserving the ability to override detection results.

## Scope

### In Scope
- Auto-detection of BDO installations from:
  - Windows Registry: `HKLM\SOFTWARE\Wow6432Node\BlackDesert_ID\Path` (standalone NA/EU client)
  - Steam library manifests: `appmanifest_836620.acf` in common Steam library folders
  - Common filesystem paths as fallback
- UI enhancements to display all detected installations with checkboxes for selection
- Logic to enable/disable the "Use selected font" button based on selection state
- Patching logic that applies to all checked paths when multiple selections are made
- Preservation of manual "Browse" button as override option

### Out of Scope
- Detection of other game versions (KR, JP, etc.) beyond NA/EU and Steam versions
- Automatic detection of custom or non-standard installation paths beyond predefined fallbacks
- Integration with package managers or third-party launchers (beyond Steam)
- Changes to font patching algorithms or core functionality
- Support for non-Windows operating systems

## Impact Assessment

### Files to Change/Create
1. **Universal Font Patcher BDO/Patcher.cs** — Main logic changes:
   - Add auto-detection methods for registry, Steam, and filesystem paths
   - Modify folder selection logic to handle multiple detected paths
   - Update UI event handlers for checkbox interactions
   - Adjust patching logic to iterate over selected paths

2. **Universal Font Patcher BDO/Patcher.Designer.cs** — UI changes:
   - Add controls for displaying detected installations (Panel with dynamic CheckBox controls)
   - Modify existing FolderBrowserDialog invocation to be optional
   - Update button enable/disable logic

3. **Universal Font Patcher BDO/Program.cs** — Minimal or no changes

### Risk Level
**Low to Moderate**
- **Low Risk**: Detection logic is read-only (registry/filesystem)
- **Moderate Risk**: UI changes involve dynamic control creation
- **Mitigation**: Safe registry access with exception handling; limit filesystem searches to predefined paths; preserve manual browse fallback

## Approach

1. **Tiered Detection Strategy**:
   - First: Registry lookup for standalone client
   - Second: Steam library parsing for `appmanifest_836620.acf`
   - Third: Common filesystem paths as fallback
   - Each method returns a list of valid BDO paths

2. **UI Implementation**:
   - On form load, run detection and populate a container with CheckBox controls
   - Each checkbox shows the detected path
   - "Browse" button remains available for manual override
   - "Use selected font" button enabled state bound to checkbox selection

3. **Selection Handling**:
   - Users can select zero, one, or multiple detected installations
   - When patching, iterate over all selected paths and apply font patches

4. **Error Handling**:
   - Detection failures don't halt application
   - If no paths detected, show informative message and enable manual browse
   - Invalid paths filtered out

## Success Criteria
- [ ] Auto-detection finds BDO installations from registry when present
- [ ] Auto-detection finds BDO installations from Steam manifests when present
- [ ] UI displays all detected installations with checkboxes
- [ ] "Use selected font" button disabled when no checkboxes are selected
- [ ] "Use selected font" button enabled when one or more checkboxes are selected
- [ ] When multiple installations selected, patching applies to all selected paths
- [ ] Manual "Browse" button always functions as override
- [ ] No regression in existing functionality when detection fails
- [ ] Application handles edge cases gracefully

## Dependencies
- .NET 8.0 Windows Forms framework
- `Microsoft.Win32` namespace for Registry access
- `System.IO` for filesystem operations
- No external NuGet packages
