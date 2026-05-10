# Archive Report: Auto-Detect BDO Game Path

## Change Summary
Implemented automatic detection of Black Desert Online game installations from multiple sources (Windows Registry, Steam library manifests, and common filesystem paths) to eliminate manual path selection while preserving override capability.

## Files Changed/Created
1. **Created**: `Universal Font Patcher BDO/BdoPathDetector.cs` (235 lines)
   - Static class with tiered detection pipeline (registry → Steam → common paths)
   - Path validation requiring BlackDesertLauncher.exe or prestringtable/ directory
   - HashSet-based deduplication and exception handling

2. **Modified**: `Universal Font Patcher BDO/Patcher.cs` (+60 lines)
   - Added Form1_Load event handler for detection triggering
   - Implemented PathCheckbox_CheckedChanged for button state management
   - Updated BtnContinue_Click for multi-path iteration with per-path error handling
   - Updated BtnSelectGameFolder_Click to add browsed paths as checkboxes
   - Added per-path result display via MessageBox summary

3. **Modified**: `Universal Font Patcher BDO/Patcher.Designer.cs` (+40 lines)
   - Added FlowLayoutPanel for dynamic checkbox controls
   - Added label for detected installations header
   - Adjusted form size and control positioning
   - Wire Load event and initialize new controls

## Verification Result
**PASS** - All functional and non-functional requirements satisfied:
- FR-1: Detect BDO installations from registry, Steam, and common paths ✓
- FR-2: Display detected installations as checkboxes with scrollable area ✓
- FR-3: Selection behavior enables/disables patch button appropriately ✓
- FR-4: Patching applies to all selected paths with error continuation ✓
- NFR-1: Detection completes within 2 seconds ✓
- NFR-2: Registry access wrapped in try/catch ✓
- NFR-3: No external NuGet packages ✓
- NFR-4: Runs on Windows 10/11 (net8.0-windows) ✓

## Known Limitations & Notes
- Detection focused on NA/EU and Steam versions only (excludes KR/JP clients)
- Steam detection includes primary library and secondary libraries via libraryfolders.vdf
- Common paths fallback includes 5 hardcoded locations across C:/ and D:/ drives
- Manual browse always available as override and adds to detection list
- Existing TxtGamePath preserved as read-only count summary ("N installation(s) detected")
- No changes to core font patching algorithm or functionality
- Dark theme styling preserved with white text on transparent backgrounds
- Sound effects maintained: button_sound for full success, error_sound for any failure

## Implementation Notes
- Detection runs on every form load via Form1.Load event
- Checkboxes show display name ("Steam"/"Standalone") plus full path
- Browse-added paths prefixed with "Manual:" in checkbox text
- Per-path errors collected and displayed in aggregated MessageBox
- Deduplication uses StringComparer.OrdinalIgnoreCase across all detection methods
- Registry access includes both 64-bit and 32-bit hive fallbacks
- Validation requires either BlackDesertLauncher.exe OR prestringtable/ directory existence