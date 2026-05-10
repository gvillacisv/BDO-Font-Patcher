# Tasks: Auto-Detect BDO Game Path

## Prerequisites
- [x] Read existing code to understand current flow (Patcher.cs, Patcher.Designer.cs)
- [x] Read specs and design from engram (observation #220, #221)

## Task Group 1: Detection Layer (BdoPathDetector.cs)
- [x] **1.1** Create `BdoPathDetector.cs` in project root with static class definition
  - Add `using System.IO;` and `using Microsoft.Win32;`
  - Define `public static List<string> DetectAll()` method
  - Define private helpers: `DetectFromRegistry()`, `DetectFromSteam()`, `DetectFromCommonPaths()`
  - Define `private static bool IsValidBdoPath(string path)` helper
  - Reference: Design section "Interfaces / Contracts"

- [x] **1.2** Implement registry detection (`DetectFromRegistry()`)
  - Query `HKLM\SOFTWARE\Wow6432Node\BlackDesert_ID\Path` registry key
  - Fallback to `HKLM\SOFTWARE\BlackDesert_ID\Path` (32-bit)
  - Return list of found paths (use HashSet<string> to dedupe)

- [x] **1.3** Implement Steam manifest detection (`DetectFromSteam()`)
  - Find Steam installation: `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam\InstallPath`
  - Parse `steamapps/appmanifest_836620.acf` (BDO app ID: 836620)
  - Extract `installdir` value from manifest
  - Construct full path: `{SteamPath}/steamapps/common/{installdir}`
  - Also parse `libraryfolders.vdf` for secondary Steam libraries

- [x] **1.4** Implement common paths fallback (`DetectFromCommonPaths()`)
  - Hardcoded paths: `C:\Pearl Abyss\BlackDesert`, `C:\Program Files (x86)\Black Desert Online`, `C:\Program Files\Black Desert Online`, `D:\Pearl Abyss\BlackDesert`, `D:\Games\Pearl Abyss\BlackDesert`
  - Check all drives

- [x] **1.5** Implement path validation (`IsValidBdoPath(string path)`)
  - Return true if path contains `BlackDesertLauncher.exe` OR contains `prestringtable/` subdirectory
  - Return false for null/empty/whitespace paths
  - Return false if Directory.Exists(path) is false

## Task Group 2: UI Changes (Patcher.Designer.cs)
- [ ] **2.1** Add `FlowLayoutPanel` for detected paths checkboxes
  - Add field: `private System.Windows.Forms.FlowLayoutPanel flowPathsPanel;`
  - Initialize in `InitializeComponent()`: `this.flowPathsPanel = new System.Windows.Forms.FlowLayoutPanel();`
  - Position: below `TxtFontPath` (around y=125), x=12, width=459, height=80
  - Set `FlowDirection = TopDown`, `WrapContents = false`, `AutoScroll = true`
  - Add to Controls collection

- [ ] **2.2** Add "Detected installations:" label above panel
  - Add field: `private System.Windows.Forms.Label lblDetectedPaths;`
  - Position: above flowPathsPanel, x=12, y=105
  - Text: "Detected installations:", ForeColor: White, Font: Calibri 9pt

- [ ] **2.3** Wire Form1.Load event handler
  - In `InitializeComponent()`: `this.Load += new System.EventHandler(this.Form1_Load);`

- [ ] **2.4** Adjust form height for new controls
  - Increase form height: Current 182 → New ~270
  - Update `this.ClientSize = new System.Drawing.Size(478, 270);`
  - Shift `BtnContinue` down to below checkbox panel (y ~215)
  - Shift `gunaLabel3` footer down accordingly

- [ ] **2.5** Add field declaration for label (if not already present)
  - Add to field declarations section at bottom of Designer.cs

## Task Group 3: Integration (Patcher.cs)
- [ ] **3.1** Add Form1.Load event handler (`Form1_Load`)
  - Call `BdoPathDetector.DetectAll()` on form load
  - Iterate results and create CheckBox for each valid path
  - Add each CheckBox to `flowPathsPanel.Controls`
  - Wire `CheckedChanged` event to `PathCheckbox_CheckedChanged`
  - Set default: if any detected, check first one by default
  - Update `TxtGamePath.Text` to show "N installation(s) detected" summary

- [ ] **3.2** Add checkbox checked-changed handler (`PathCheckbox_CheckedChanged`)
  - Update `BtnContinue.Enabled`: enabled when (any checkbox checked AND font selected)
  - Get font path from `TxtFontPath.Text`
  - Use pattern: `flowPathsPanel.Controls.OfType<CheckBox>().Any(cb => cb.Checked)`

- [ ] **3.3** Update `BtnContinue_Click` for multi-path iteration
  - Change: iterate over all CheckBoxes in flowPathsPanel where Checked == true
  - For each checked path: perform font copy to `{path}\prestringtable\font\pearl.ttf`
  - Wrap each iteration in try/catch (per-path error handling)
  - Show per-path result: success message or error message per path
  - Play success sound only if ALL paths succeed; play error if ANY fail

- [ ] **3.4** Update `BtnSelectGameFolder_Click` to add browsed path as checkbox
  - After validation (contains "BlackDesert"), create new CheckBox with path as Text
  - Add to flowPathsPanel.Controls with Checked = true
  - Update TxtGamePath summary count
  - Call `PathCheckbox_CheckedChanged` to update BtnContinue state

- [ ] **3.5** Add per-path result display logic
  - Collect results in Dictionary<string, bool> (path → success)
  - After loop: show MessageBox summary like "Patched: 2/3 paths" with details
  - Or: Show individual results inline (e.g., label showing last operation result)

## Task Group 4: Polish
- [ ] **4.1** Handle edge cases
  - Zero detected paths: Show "No BDO installations found. Use Browse to select manually." in lblDetectedPaths
  - Permission errors: Catch UnauthorizedAccessException, show friendly message
  - Path no longer exists: Skip gracefully, show "Path not found: {path}"
  - Font not selected: BtnContinue stays disabled until TxtFontPath has value

- [ ] **4.2** Final review and testing
  - Verify form loads without errors
  - Verify detection works (registry, Steam, common paths)
  - Verify multi-select works (select 2+ checkboxes)
  - Verify browse adds new checkbox correctly
  - Verify font patching works to multiple paths
  - Verify error handling for invalid paths

## File Summary
| File | Action | Lines (est) |
|------|--------|-------------|
| `BdoPathDetector.cs` | Create | ~100 lines |
| `Patcher.cs` | Modify | +60 lines |
| `Patcher.Designer.cs` | Modify | +40 lines |

## Dependencies
- Task 1.x must complete before Task 3.x (detection logic needed for integration)
- Task 2.x must complete before Task 3.x (UI controls needed for wiring)
- Task 4.x is final verification after all integration is done

## Done Conditions
- **1.x**: BdoPathDetector.cs compiles, DetectAll() returns List<string>
- **2.x**: Form displays FlowLayoutPanel with label, form resized correctly
- **3.x**: Form loads detect paths, checkboxes toggle BtnContinue, multi-path patching works
- **4.x**: All edge cases handled, manual testing passes