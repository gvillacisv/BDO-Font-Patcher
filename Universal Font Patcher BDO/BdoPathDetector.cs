using Microsoft.Win32;
using System.Security;

namespace Universal_Font_Patcher_BDO;

internal static class BdoPathDetector
{
    private static readonly string[] UninstallRoots =
    {
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
    };

    /// <summary>
    /// Detects BDO installations by scanning Windows Uninstall registry entries
    /// whose subkey name starts with "BlackDesert_" (e.g., BlackDesert_NA_is1,
    /// BlackDesert_SA_is1, BlackDesert_EU_is1) or whose DisplayName contains
    /// "Black Desert". Returns all valid installation paths found.
    /// </summary>
    public static List<string> DetectAll()
    {
        var paths = new List<string>();

        foreach (string root in UninstallRoots)
        {
            try
            {
                using var uninstallKey = Registry.LocalMachine.OpenSubKey(root, false);
                if (uninstallKey == null) continue;

                foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                {
                    try
                    {
                        using var appKey = uninstallKey.OpenSubKey(subKeyName, false);
                        if (appKey == null) continue;

                        bool isBdo = false;

                        // Priority: subkey named "BlackDesert_*" (NA, SA, EU, etc.)
                        if (subKeyName.StartsWith("BlackDesert_", StringComparison.OrdinalIgnoreCase))
                            isBdo = true;

                        // Fallback: DisplayName contains "Black Desert"
                        if (!isBdo)
                        {
                            string? displayName = appKey.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(displayName) &&
                                (displayName.Contains("Black Desert", StringComparison.OrdinalIgnoreCase) ||
                                 displayName.Contains("BlackDesert", StringComparison.OrdinalIgnoreCase)))
                                isBdo = true;
                        }

                        if (!isBdo) continue;

                        // Read InstallLocation (Inno Setup entries always have this)
                        string? installLoc = appKey.GetValue("InstallLocation") as string;
                        if (!string.IsNullOrEmpty(installLoc) && IsValidBdoPath(installLoc) && !paths.Contains(installLoc))
                        {
                            paths.Add(installLoc);
                            continue;
                        }

                        // Fallback: derive path from DisplayIcon
                        string? iconPath = appKey.GetValue("DisplayIcon") as string;
                        if (!string.IsNullOrEmpty(iconPath))
                        {
                            string? dir = Path.GetDirectoryName(iconPath);
                            if (!string.IsNullOrEmpty(dir) && IsValidBdoPath(dir) && !paths.Contains(dir))
                                paths.Add(dir);
                        }
                    }
                    catch (SecurityException) { }
                    catch (UnauthorizedAccessException) { }
                }
            }
            catch (SecurityException) { }
            catch (UnauthorizedAccessException) { }
        }

        return paths;
    }

    public static bool IsValidBdoPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        path = path.TrimEnd();

        if (path.IndexOf('\0') >= 0)
            return false;

        if (!Directory.Exists(path))
            return false;

        string launcherPath = Path.Combine(path, "BlackDesertLauncher.exe");
        string prestringPath = Path.Combine(path, "prestringtable");

        return File.Exists(launcherPath) || Directory.Exists(prestringPath);
    }
}
