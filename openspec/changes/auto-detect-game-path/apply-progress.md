# Apply Progress: auto-detect-game-path — Task Group 1

## Completed Tasks
- [x] **1.1** Create `BdoPathDetector.cs` — static class with DetectAll() + all private helpers
- [x] **1.2** Registry detection — HKLM\Wow6432Node\BlackDesert_ID\Path + 32-bit fallback, HashSet dedup
- [x] **1.3** Steam detection — Steam registry path → appmanifest_836620.acf + libraryfolders.vdf parsing
- [x] **1.4** Common paths fallback — 5 hardcoded paths, IsValidBdoPath filter
- [x] **1.5** Path validation — null/empty guard, Directory.Exists, BlackDesertLauncher.exe OR prestringtable/

## Files Changed
| File | Action | What Was Done |
|------|--------|---------------|
| `Universal Font Patcher BDO/BdoPathDetector.cs` | Created | 235-line static class with full detection pipeline |
| `openspec/changes/auto-detect-game-path/tasks.md` | Modified | Marked 1.1–1.5 as complete |

## Implementation Summary

`BdoPathDetector.cs` — internal static class, `net8.0-windows` implicit usings.

### Detection Pipeline (DetectAll):
1. **Registry** → Try 64-bit HKLM\SOFTWARE\Wow6432Node\BlackDesert_ID\Path, fallback to 32-bit
2. **Steam** → Get Steam install path from HKLM\SOFTWARE\WOW6432Node\Valve\Steam, check primary + manifest + libraryfolders
3. **Common** → 5 hardcoded paths
4. **Dedup** → HashSet<string> with OrdinalIgnoreCase, return List<string>

### IsValidBdoPath criteria:
- Not null/empty/whitespace
- Directory.Exists must be true
- File.Exists(BlackDesertLauncher.exe) OR Directory.Exists(prestringtable)

### Exception handling:
- Registry: SecurityException + SystemException wrapped silently
- Steam/libraryfolders: top-level catch-all Exception
- No external NuGet packages

## Deviations from Design
- `internal static` (not `public static`) — scoped to project, reduces surface area
- Added `DetectFromSteamLibraryFolders()` helper (design deferred but implemented for completeness)
- Added `TryReadRegistryPath()` helper to reduce duplication

## Remaining Tasks (Groups 2–4)
- Task Group 2: UI changes (Patcher.Designer.cs) — FlowLayoutPanel, label, form resize
- Task Group 3: Integration (Patcher.cs) — Form1_Load, checkbox wiring, multi-path patching
- Task Group 4: Polish — edge cases, final review

---

# Apply Progress: auto-detect-game-path — Task Group 3

## Completed Tasks
- [x] **3.1** Form1_Load event handler — Detects all BDO paths on form load, populates flowPathsPanel with checkboxes (Steam/Standalone labels)
- [x] **3.2** PathCheckbox_CheckedChanged handler — Enables BtnContinue only when a path is checked AND a font is selected
- [x] **3.3** BtnContinue_Click updated — Iterates all checked paths, copies font to each, tracks success/fail counts with per-path error messages
- [x] **3.4** BtnSelectGameFolder_Click updated — Adds browsed path as new checkbox, checks existing if already present, updates count display
- [x] **3.5** Per-path result display — MessageBox shows success/fail counts and per-path error details

## Files Changed
| File | Action | What Was Done |
|------|--------|---------------|
| `Universal Font Patcher BDO/Patcher.cs` | Modified | Added using System.Linq, Form1_Load, PathCheckbox_CheckedChanged, updated BtnContinue_Click and BtnSelectGameFolder_Click |

## Implementation Details

### Form1_Load (lines 20-49)
```csharp
private void Form1_Load(object? sender, EventArgs e)
{
    var paths = BdoPathDetector.DetectAll();

    if (paths.Count == 0)
    {
        lblDetectedPaths.Text = "No BDO installations found. Use Browse to select manually.";
        BtnContinue.Enabled = false;
        return;
    }

    foreach (var path in paths)
    {
        var cb = new CheckBox();
        string displayName = path.Contains("Steam", StringComparison.OrdinalIgnoreCase)
            ? "Steam"
            : "Standalone";
        cb.Text = $"{displayName}: {path}";
        cb.Tag = path;
        cb.ForeColor = Color.White;
        cb.BackColor = Color.Transparent;
        cb.Font = new Font("Calibri", 9F, FontStyle.Regular);
        cb.CheckedChanged += PathCheckbox_CheckedChanged;
        cb.Checked = true;
        flowPathsPanel.Controls.Add(cb);
    }

    TxtGamePath.Text = $"{paths.Count} installation(s) detected";
    PathCheckbox_CheckedChanged(sender, e);
}
```

### PathCheckbox_CheckedChanged (lines 51-60)
```csharp
private void PathCheckbox_CheckedChanged(object? sender, EventArgs? e)
{
    bool anyChecked = flowPathsPanel.Controls
        .OfType<CheckBox>()
        .Any(cb => cb.Checked);

    bool fontSelected = !string.IsNullOrWhiteSpace(TxtFontPath.Text);

    BtnContinue.Enabled = anyChecked && fontSelected;
}
```

### BtnContinue_Click (lines 68-116)
- Iterates all checked checkboxes from flowPathsPanel
- Creates destination directory: `{gamePath}\prestringtable\font\`
- Copies font to `pearl.ttf` in each directory
- Tracks successCount and failCount
- Per-path errors collected in `List<string>`
- **Sound**: button_sound plays only when ALL succeed; error_sound plays when ANY fail

### BtnSelectGameFolder_Click (lines 127-188)
- After validation passes, checks if path already exists in flowPathsPanel
- If not exists: adds new checkbox with "Manual:" prefix
- If exists: checks the existing checkbox
- Updates TxtGamePath.Text to show count: `{count} installation(s)`

## Preserved Functionality
- Original checkbox_sound on UI interactions
- Original button_sound on full success
- Original error_sound on partial/total failure
- Dark theme styling (Color.White, Color.Transparent, Font "Calibri", 9F)
- Original folder validation logic ("BlackDesert" check)
- Original error message: "The selected folder doesn't seem to be the proper one..."

## Remaining Tasks
- Task Group 4: Polish — edge cases, final review