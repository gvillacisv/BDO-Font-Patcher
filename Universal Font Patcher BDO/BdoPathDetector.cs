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

    public static List<string> DetectAll()
    {
        var results = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in DetectFromRegistry())
            results.Add(path);

        foreach (var path in DetectFromSteam())
            results.Add(path);

        return results.ToList();
    }

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
            // Insufficient permissions — skip silently
            return null;
        }
        catch (SystemException)
        {
            // Key not found or other registry error — skip silently
            return null;
        }
    }

    private static List<string> DetectFromSteam()
    {
        var paths = new List<string>();

        try
        {
            // Get Steam install path from registry
            string? steamPath = GetSteamInstallPath();
            if (string.IsNullOrEmpty(steamPath) || !Directory.Exists(steamPath))
                return paths;

            // Collect all Steam library roots (primary + secondary from libraryfolders.vdf)
            var libraryRoots = GetSteamLibraryRoots(steamPath);

            // Scan each library's steamapps/common/ for BDO installations
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
            // Steam not installed or inaccessible — skip silently
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
            // Corrupt libraryfolders.vdf — skip, primary path already added
        }

        return roots;
    }

    public static bool IsValidBdoPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        // Trim trailing whitespace and resolve any malformed path components
        path = path.TrimEnd();

        // Reject paths with invalid characters (e.g., a literal null char)
        if (path.IndexOf('\0') >= 0)
            return false;

        if (!Directory.Exists(path))
            return false;

        // Must contain BlackDesertLauncher.exe OR prestringtable subdirectory
        string launcherPath = Path.Combine(path, "BlackDesertLauncher.exe");
        string prestringPath = Path.Combine(path, "prestringtable");

        return File.Exists(launcherPath) || Directory.Exists(prestringPath);
    }
}