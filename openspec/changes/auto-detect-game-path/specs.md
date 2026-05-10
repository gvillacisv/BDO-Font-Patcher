# Specs: Auto-Detect BDO Game Path

## Functional Requirements

### FR-1: Detect BDO installations
- [ ] FR-1.1: On form load, scan for BDO installations
- [ ] FR-1.2: Scan Windows Registry at HKLM\SOFTWARE\Wow6432Node\BlackDesert_ID\Path
- [ ] FR-1.3: Scan Steam manifests for app ID 836620
- [ ] FR-1.4: Scan common filesystem paths
- [ ] FR-1.5: Validate each candidate path (must contain BlackDesertLauncher.exe)
- [ ] FR-1.6: Return list of 0, 1, or N valid BDO paths

### FR-2: Display detected installations
- [ ] FR-2.1: Show detected paths as checkboxes
- [ ] FR-2.2: Each checkbox shows the display name + full path
- [ ] FR-2.3: If 0 detected, show "Not found — click Browse to select" message
- [ ] FR-2.4: If no search results, scrollable area if many

### FR-3: Selection behavior
- [ ] FR-3.1: User can check/uncheck any detected installation
- [ ] FR-3.2: "Use selected font" button disabled when 0 checked
- [ ] FR-3.3: "Use selected font" enabled when >= 1 checked
- [ ] FR-3.4: Manual Browse adds a path to the list (checked by default)

### FR-4: Patching with multiple paths
- [ ] FR-4.1: Clicking "Use selected font" patches ALL checked paths
- [ ] FR-4.2: Each path gets font copied to {path}\prestringtable\font\pearl.ttf
- [ ] FR-4.3: Show success/failure per path (e.g., "✓ NA: patched", "✓ SA: patched")
- [ ] FR-4.4: If one path fails, continue with remaining paths (don't stop on error)

## Non-Functional Requirements
- NFR-1: Detection must complete within 2 seconds
- NFR-2: Registry access wrapped in try/catch for SecurityException
- NFR-3: No external NuGet packages
- NFR-4: Must run on Windows 10/11 (target: win-x64)

## Out of Scope
- Detection of KR/JP/other regional clients
- BDO reinstallation or repair features
- Changing the font patching algorithm itself
- Support for non-Windows OS