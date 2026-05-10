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
    private const string BdoSteamAppId = "836620";

    private static readonly string[] CommonPaths =
    {
        @"C:\Pearl Abyss\BlackDesert",
        @"C:\Program Files (x86)\Black Desert Online",
        @"C:\Program Files\Black Desert Online",
        @"D:\Pearl Abyss\BlackDesert",
        @"D:\Games\Pearl Abyss\BlackDesert"
    };

    public static List<string> DetectAll()
    {
        var results = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in DetectFromRegistry())
            results.Add(path);

        foreach (var path in DetectFromSteam())
            results.Add(path);

        foreach (var path in DetectFromCommonPaths())
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

            // Check primary library: {SteamPath}\steamapps\common\BlackDesert
            string primaryPath = Path.Combine(steamPath, "steamapps", "common", "BlackDesert");
            if (IsValidBdoPath(primaryPath))
                paths.Add(primaryPath);

            // Parse appmanifest to get the actual install directory name (handles renames)
            string manifestPath = Path.Combine(steamPath, "steamapps", $"appmanifest_{BdoSteamAppId}.acf");
            string? installDir = ParseSteamManifestInstallDir(manifestPath);
            if (!string.IsNullOrEmpty(installDir))
            {
                string manifestInstallPath = Path.Combine(steamPath, "steamapps", "common", installDir);
                if (IsValidBdoPath(manifestInstallPath) && !paths.Contains(manifestInstallPath))
                    paths.Add(manifestInstallPath);
            }

            // Check secondary Steam library folders
            paths.AddRange(DetectFromSteamLibraryFolders(steamPath));
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

    private static string? ParseSteamManifestInstallDir(string manifestPath)
    {
        if (!File.Exists(manifestPath))
            return null;

        try
        {
            // Parse "installdir" from appmanifest_836620.acf
            // Format: "installdir"    "BlackDesert"
            foreach (var line in File.ReadLines(manifestPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("\"installdir\"", StringComparison.OrdinalIgnoreCase))
                {
                    int firstQuote = trimmed.IndexOf('"');
                    int secondQuote = trimmed.IndexOf('"', firstQuote + 1);
                    if (firstQuote >= 0 && secondQuote > firstQuote)
                    {
                        return trimmed.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                    }
                }
            }
        }
        catch (Exception)
        {
            // Corrupt manifest — skip
        }

        return null;
    }

    private static List<string> DetectFromSteamLibraryFolders(string steamPath)
    {
        var paths = new List<string>();

        string libraryFoldersVdf = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
        if (!File.Exists(libraryFoldersVdf))
            return paths;

        try
        {
            // Parse libraryfolders.vdf for "path" entries
            // Format: "path"    "D:\SteamLibrary"
            var libraryRoots = new List<string> { steamPath };

            foreach (var line in File.ReadLines(libraryFoldersVdf))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("\"path\"", StringComparison.OrdinalIgnoreCase))
                {
                    int firstQuote = trimmed.IndexOf('"');
                    int secondQuote = trimmed.IndexOf('"', firstQuote + 1);
                    if (firstQuote >= 0 && secondQuote > firstQuote)
                    {
                        string libPath = trimmed.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                        if (Directory.Exists(libPath) && !libraryRoots.Contains(libPath, StringComparer.OrdinalIgnoreCase))
                            libraryRoots.Add(libPath);
                    }
                }
            }

            // Check each library for BDO
            foreach (var libRoot in libraryRoots)
            {
                // Direct path
                string bdoPath = Path.Combine(libRoot, "steamapps", "common", "BlackDesert");
                if (IsValidBdoPath(bdoPath) && !paths.Contains(bdoPath))
                    paths.Add(bdoPath);

                // Parsed from manifest
                string manifestPath = Path.Combine(libRoot, "steamapps", $"appmanifest_{BdoSteamAppId}.acf");
                string? installDir = ParseSteamManifestInstallDir(manifestPath);
                if (!string.IsNullOrEmpty(installDir))
                {
                    string manifestPath2 = Path.Combine(libRoot, "steamapps", "common", installDir);
                    if (IsValidBdoPath(manifestPath2) && !paths.Contains(manifestPath2))
                        paths.Add(manifestPath2);
                }
            }
        }
        catch (Exception)
        {
            // Corrupt libraryfolders.vdf — skip
        }

        return paths;
    }

    private static List<string> DetectFromCommonPaths()
    {
        return CommonPaths
            .Where(IsValidBdoPath)
            .ToList();
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