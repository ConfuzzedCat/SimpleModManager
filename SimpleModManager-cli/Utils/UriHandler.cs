using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Serilog;
using SimpleModManager.Util;

namespace SimpleModManager_cli.Utils;

public sealed class UriHandler
{
    private static ILogger _logger;

    /// url structure
    // nxm://<game>/mods/<modid>/files/<fileid>?key=<key>&expires=<time>&user_id=<userid>
    // Example:
    // nxm://cyberpunk2077/mods/16349/files/85356?key=6SS...d4cA&expires=1724532165&user_id=14184246
    // for: https://www.nexusmods.com/cyberpunk2077/mods/16349?tab=files
    static UriHandler()
    {
        _logger = LoggerHandler.GetLogger<UriHandler>();
    }

    //TODO: Add remove uri handlers option

    public static void AddUriSchemeHandler()
    {
        var uriFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".uri_handled");
        if (File.Exists(uriFile)) return;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) AddUriHandlerLinux();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) AddUriHandlerWindows();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new NotSupportedException("MacOS/OSX not supported yet.");
        File.WriteAllText(uriFile, string.Empty);
        File.SetAttributes(uriFile, FileAttributes.Hidden);
    }

    private static void AddUriHandlerWindows()
    {
        var scriptPsi = new ProcessStartInfo
        {
            Arguments = Assembly.GetExecutingAssembly().Location,
            FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "URI-Handling-Windows.bat"),
            UseShellExecute = true,
            Verb = "runas"
        };
        Process.Start(scriptPsi);
    }

    private static void AddUriHandlerLinux()
    {
        var scriptPsi = new ProcessStartInfo
        {
            Arguments = Assembly.GetExecutingAssembly().Location,
            FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "URI-Handling-Linux.sh"),
            UseShellExecute = true
        };
        Process.Start(scriptPsi);
    }
}