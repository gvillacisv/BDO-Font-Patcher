# Exploration: Auto-Detect BDO Game Path

> Generated: 2026-05-09
> Persistence: hybrid (engram + openspec)

## Current Behavior

The app uses a WinForms UI with two read-only text fields and browse buttons. Path selection works as follows (from `Patcher.cs`):

1. User clicks "Select game folder" → `FolderBrowserDialog` opens
2. App checks: does selected path contain the substring `"BlackDesert"`?
3. If yes → populates `TxtGamePath.Text`
4. If no → error: "The selected folder doesn't seem to be the proper one"

**Critical issue:** This validation only checks for the substring `"BlackDesert"` in the path string — it does **not** verify that BDO files exist.

The patching code uses the path as: `TxtGamePath.Text + "\\prestringtable\\font\\pearl.ttf"`, meaning it expects the **root BDO install folder**.

## Detection Strategies Evaluated

### Strategy 1: Windows Registry — `HKLM\SOFTWARE\Wow6432Node\BlackDesert_ID`
**Reliability: HIGH**

BDO writes its install path to this key during installation. Works for both standalone and Steam BDO.

| Pros | Cons |
|------|------|
| Most reliable single source; set at install time | Registry value can be stale if user moved BDO |
| Works without Steam client running | Must validate the path exists |
| Non-invasive, read-only | |

### Strategy 2: Steam Library Manifest Parsing
**Reliability: HIGH for Steam users**

BDO Steam App ID: **836620**. Manifest file: `steamapps/appmanifest_836620.acf`, containing `installDir` field.

| Pros | Cons |
|------|------|
| Very reliable for Steam installations | Only detects Steam-installed BDO |
| Handles library moves within Steam | Requires parsing `.acf` format |
| Works with multiple Steam library folders | Steam client must be installed |

### Strategy 3: Common Filesystem Paths
**Reliability: MEDIUM**

Known locations: `C:\Pearl Abyss\BlackDesert`, `C:\Program Files (x86)\Black Desert Online`, etc.

### Strategy 4: Windows Uninstall Registry Key
**Reliability: LOW-MEDIUM**

## Recommended Approach

**Tiered detection with validation** — try each source, accumulate all valid results:

```
1. Registry (HKLM\SOFTWARE\Wow6432Node\BlackDesert_ID\Path)  ← Standalone client
2. Steam manifest (appmanifest_836620.acf)                    ← Steam client
3. Common filesystem paths                                     ← Fallback
```

## User Feedback Incorporated

- Multiple selection UI with checkboxes for each detected path
- User can select 1, both, or none
- "Use selected font" disabled when nothing is checked
- If multiple selected, patching applies to all checked paths
- Manual "Browse" override always available

## Risk Mitigation

| Risk | Mitigation |
|------|-----------|
| Stale registry | Validate with `File.Exists(BlackDesertLauncher.exe)` |
| Multiple BDO installs | Show all, let user choose |
| Registry access denied | Graceful fallback to next strategy |
| Steam present but BDO not Steam | Steam returns null → fallthrough |
| No detection | Manual browse always works |
