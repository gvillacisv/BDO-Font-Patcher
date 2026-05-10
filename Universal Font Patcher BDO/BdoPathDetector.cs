using Microsoft.Win32;
using System.Security;

namespace Universal_Font_Patcher_BDO;

internal static class BdoPathDetector
{
    private const string BdoRegistryKey64 = @"SOFTWARE\Wow6432Node\BlackDesert_ID";
    private const string BdoRegistryKey32 = @"SOFTWARE\BlackDesert_ID";
    private const string BdoRegistryValueName = "Path";

    private const string SteamRegistryKey = @"SOFTWARE\WOW6432Node\Valve\Steam";
    private const string SteamInstallPathValue = "InstallPath";

    private static readonly string[] UninstallRoots =
    {
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
    };

    public static List<string> DetectAll()
    {
        var results = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in DetectFromRegistry())
            results.Add(path);

        foreach (var path in DetectFromUninstall())
            results.Add(path);

        foreach (var path in DetectFromSteam())
            results.Add(path);

        return results.ToList();
    }

    /// <summary>
    /// Detection 1: BlackDesert_ID registry key (official BDO installer).
    /// </summary>
    private static List<string> DetectFromRegistry()
    {
        var paths = new List<string>();

        // Try 64-bit view first
        string? path = TryReadRegistryPath(Registry.LocalMachine, BdoRegistryKey64);
        if (!string.IsNullOrEmpty(path))
            paths.Add(path);

        // Fallback to 32-bit view
        path = TryReadRegistryPath(Registry.LocalMachine, BdoRegistryKey32);
        if (!string.IsNullOrEmpty(path) && !paths.Contains(path))
            paths.Add(path);

        return paths;
    }

    private static string? TryReadRegistryPath(RegistryKey baseKey, string subKeyPath)
    {
        try
        {
            using var subKey = baseKey.OpenSubKey(subKeyPath, false);
            return subKey?.GetValue(BdoRegistryValueName) as string;
        }
        catch (SecurityException)
        {
            return null;
        }
        catch (SystemException)
        {
            return null;
        }
    }

    /// <summary>
    /// Detection 2: Windows Uninstall registry entries for "Black Desert".
    /// Covers both old Daum and new Pearl Abyss installers.
    /// </summary>
    private static List<string> DetectFromUninstall()
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

                        // Priority: match subkey name "BlackDesert_*" (consistent across all regions)
                        if (subKeyName.StartsWith("BlackDesert_", StringComparison.OrdinalIgnoreCase))
                            isBdo = true;

                        // Fallback: match DisplayName containing "Black Desert"
                        if (!isBdo)
                        {
                            string? displayName = appKey.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(displayName) &&
                                (displayName.Contains("Black Desert", StringComparison.OrdinalIgnoreCase) ||
                                 displayName.Contains("BlackDesert", StringComparison.OrdinalIgnoreCase)))
                                isBdo = true;
                        }

                        if (!isBdo) continue;

                        // Try InstallLocation first (Inno Setup entries always have this)
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
                    catch (Exception)
                    {
                        // Skip individual uninstall entries that can't be read
                    }
                }
            }
            catch (Exception)
            {
                // Skip uninstall root if inaccessible
            }
        }

        return paths;
    }

    /// <summary>
    /// Detection 3: Steam libraries — scan steamapps/common/ for BDO installations.
    /// </summary>
    private static List<string> DetectFromSteam()
    {
        var paths = new List<string>();

        try
        {
            string? steamPath = GetSteamInstallPath();
            if (string.IsNullOrEmpty(steamPath) || !Directory.Exists(steamPath))
                return paths;

            var libraryRoots = GetSteamLibraryRoots(steamPath);

            foreach (var libRoot in libraryRoots)
            {
                string commonDir = Path.Combine(libRoot, "steamapps", "common");
                if (!Directory.Exists(commonDir))
                    continue;

                foreach (string subDir in Directory.EnumerateDirectories(commonDir))
                {
                    if (IsValidBdoPath(subDir) && !paths.Contains(subDir))
                        paths.Add(subDir);
                }
            }
        }
        catch (Exception)
        {
            // Steam not installed or inaccessible
        }

        return paths;
    }

    private static string? GetSteamInstallPath()
    {
        try
        {
            using var steamKey = Registry.LocalMachine.OpenSubKey(SteamRegistryKey, false);
            return steamKey?.GetValue(SteamInstallPathValue) as string;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static List<string> GetSteamLibraryRoots(string primarySteamPath)
    {
        var roots = new List<string> { primarySteamPath };

        string vdfPath = Path.Combine(primarySteamPath, "steamapps", "libraryfolders.vdf");
        if (!File.Exists(vdfPath))
            return roots;

        try
        {
            foreach (var line in File.ReadLines(vdfPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("\"path\"", StringComparison.OrdinalIgnoreCase))
                {
                    int firstQuote = trimmed.IndexOf('"');
                    int secondQuote = trimmed.IndexOf('"', firstQuote + 1);
                    if (firstQuote >= 0 && secondQuote > firstQuote)
                    {
                        string libPath = trimmed.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                        if (Directory.Exists(libPath) &&
                            !roots.Contains(libPath, StringComparer.OrdinalIgnoreCase))
                        {
                            roots.Add(libPath);
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            // Corrupt libraryfolders.vdf — skip
        }

        return roots;
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
