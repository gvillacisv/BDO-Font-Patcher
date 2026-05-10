# Design: Auto-Detect BDO Game Path

## Technical Approach

Add a tiered detection pipeline that discovers BDO installations from three independent sources (registry, Steam, common paths). Results populate a dynamic checkbox panel in the form. The user selects which paths to patch; `BtnContinue` iterates over all checked paths. The existing manual browse workflow is preserved as an always-available override.

## Architecture Decisions

### Decision: Separate `BdoPathDetector` static class

| Option | Tradeoff | Decision |
|--------|----------|----------|
| Detection logic inline in `Form1` private methods | Fewer files, but couples detection to form lifecycle | ❌ |
| **New `BdoPathDetector.cs` static class** | Clean separation, testable in isolation, no state | ✅ |

Rationale: Detection is pure input (no UI state, no form references). A static utility class with no constructor/dependencies is the minimum viable abstraction — follows Karpathy simplicity.

### Decision: `Form1.Load` event for detection trigger

| Option | Tradeoff | Decision |
|--------|----------|----------|
| Constructor | Controls not fully initialized; WinForms anti-pattern | ❌ |
| **Form1.Load event** | Standard WinForms lifecycle hook; all controls ready | ✅ |

### Decision: FlowLayoutPanel with dynamic CheckBox controls

| Option | Tradeoff | Decision |
|--------|----------|----------|
| ListBox / DataGridView | More complex selection model, needs custom rendering | ❌ |
| **FlowLayoutPanel + CheckBox** | Natural multi-select UX; each checkbox is self-contained | ✅ |

### Decision: Repurpose TxtGamePath as read-only summary, add panel below

| Option | Tradeoff | Decision |
|--------|----------|----------|
| Replace TxtGamePath entirely | Breaks existing references; larger diff | ❌ |
| **TxtGamePath shows "N installation(s) detected"** | Minimal change; backward compatible | ✅ |

### Decision: Manual browse adds a checked checkbox to the panel

| Option | Tradeoff | Decision |
|--------|----------|----------|
| Replace detection list with single browsed path | Loses detected paths on manual override | ❌ |
| **Append browsed path as pre-checked checkbox** | User can combine detected + manual paths | ✅ |

## Data Flow

```
Form1.Load()
  │
  └─→ BdoPathDetector.DetectAll()
       ├─ DetectFromRegistry()    → HKLM\SOFTWARE\Wow6432Node\BlackDesert_ID\Path
       ├─ DetectFromSteam()       → Steam install path → appmanifest_836620.acf
       └─ DetectFromCommonPaths() → hardcoded well-known paths
       │
       └─ IsValidBdoPath() filter → must have BlackDesertLauncher.exe OR prestringtable/
       │
       └─ List<string> validPaths
            │
            └─→ Form1: create CheckBox per path in FlowLayoutPanel
                 │
                 ├─ CheckedChanged → BtnContinue.Enabled = (any checkbox checked && font selected)
                 │
                 ├─ BtnSelectGameFolder (manual browse)
                 │   └─→ Validate path → add checked CheckBox to panel
                 │
                 └─ BtnContinue.Click()
                      └─→ foreach checked CheckBox:
                            File.Copy → {path}\prestringtable\font\pearl.ttf
                      └─→ Show per-path result
```

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `Universal Font Patcher BDO/BdoPathDetector.cs` | **Create** | Static class: `DetectAll()`, `DetectFromRegistry()`, `DetectFromSteam()`, `DetectFromCommonPaths()`, `IsValidBdoPath()` |
| `Universal Font Patcher BDO/Patcher.cs` | **Modify** | Add `Form1.Load` handler, update `BtnContinue_Click` for multi-path iteration, update `BtnSelectGameFolder_Click` to add to checkbox panel |
| `Universal Font Patcher BDO/Patcher.Designer.cs` | **Modify** | Add `LblDetectedInstallations` label, `FlowLayoutPanel` for checkboxes, wire `Load` event, enlarge form, shift existing controls down |

## Interfaces / Contracts

```csharp
internal static class BdoPathDetector
{
    /// <summary>Run all detection tiers, return deduplicated validated paths.</summary>
    public static List<string> DetectAll();

    private static List<string> DetectFromRegistry();   // RegistryKey.OpenBaseKey(...)
    private static List<string> DetectFromSteam();      // Registry → Steam path → .acf
    private static List<string> DetectFromCommonPaths(); // Well-known hardcoded dirs

    /// <summary>True if path contains BlackDesertLauncher.exe or prestringtable/ dir.</summary>
    private static bool IsValidBdoPath(string path);
}
```

## Testing Strategy

| Layer | What | Approach |
|-------|------|----------|
| Unit | `IsValidBdoPath()` | Test with known-good path, path missing exe, null/empty path |
| Unit | `DetectAll()` with mocked registry | Use `RegistryKey` test seam or conditional compile for test mode |
| Manual | UI checkbox interaction | Launch app, verify checkboxes appear, BtnContinue toggles enabled state |
| Manual | Multi-path patching | Select 2+ checkboxes, verify font copied to all paths |
| Manual | Browse fallback | Click "Select game folder" with no detections, verify path appears as checkbox |
| Manual | Error handling | Run on machine with no BDO installed, verify graceful fallback to browse-only mode |

## Migration / Rollout

No migration required. The change is purely additive — auto-detection runs on form load and populates the new panel. If detection finds zero paths (no BDO installed, no permission), the app behaves identically to today: manual browse only. Existing `TxtGamePath` is preserved as read-only summary text.

## Open Questions

- [ ] Should browsed paths persist in the panel across sessions? (Likely no — detection re-runs on every launch)
- [ ] Should the Steam detection parse `libraryfolders.vdf` for secondary Steam libraries, or only check the primary? (Per proposal: optional/advanced — defer to implementation)
- [ ] What is the exact form height increase needed? Estimate: +80px for label + 4 checkboxes (478×262)
