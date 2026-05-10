using Microsoft.Win32;
using System.Security;

namespace Universal_Font_Patcher_BDO;

internal static class BdoPathDetector
{
    private const string UninstallRoot = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

    /// <summary>
    /// Detects BDO standalone installations by scanning 32-bit Windows Uninstall
    /// registry entries whose subkey name starts with "BlackDesert_" (e.g.,
    /// BlackDesert_NA_is1, BlackDesert_SA_is1, BlackDesert_EU_is1) or whose
    /// DisplayName contains "Black Desert".
    /// Only scans Wow6432Node to avoid Steam game entries.
    /// </summary>
    public static List<string> DetectAll()
    {
        var paths = new List<string>();

        try
        {
            using var uninstallKey = Registry.LocalMachine.OpenSubKey(UninstallRoot, false);
            if (uninstallKey != null)
                ScanUninstallSubKeys(uninstallKey, paths);
        }
        catch (SecurityException) { }
        catch (UnauthorizedAccessException) { }

        return paths;
    }

    private static void ScanUninstallSubKeys(RegistryKey uninstallKey, List<string> paths)
    {
        foreach (string subKeyName in uninstallKey.GetSubKeyNames())
        {
            try
            {
                using var appKey = uninstallKey.OpenSubKey(subKeyName, false);
                if (appKey == null) continue;

                bool isBdo = subKeyName.StartsWith("BlackDesert_", StringComparison.OrdinalIgnoreCase);

                if (!isBdo)
                {
                    string? displayName = appKey.GetValue("DisplayName") as string;
                    if (!string.IsNullOrEmpty(displayName) &&
                        (displayName.Contains("Black Desert", StringComparison.OrdinalIgnoreCase) ||
                         displayName.Contains("BlackDesert", StringComparison.OrdinalIgnoreCase)))
                        isBdo = true;
                }

                if (!isBdo) continue;

                string? installLoc = appKey.GetValue("InstallLocation") as string;
                if (!string.IsNullOrEmpty(installLoc) && IsValidBdoPath(installLoc) &&
                    !paths.Any(p => string.Equals(p, installLoc, StringComparison.OrdinalIgnoreCase)))
                {
                    paths.Add(installLoc);
                }
            }
            catch (SecurityException) { }
            catch (UnauthorizedAccessException) { }
        }
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

        return File.Exists(Path.Combine(path, "BlackDesertLauncher.exe"));
    }
}
