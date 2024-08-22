using System.Runtime.InteropServices;
using Serilog;

namespace SimpleModManager;

public static class UriHandler
{
    private static ILogger _logger; 
    
    /// url structure
    // nxm://<game>/mods/<modid>/files/<fileid>?key=<key>&expires=<time>&user_id=<userid>
    // Example:
    // nxm://cyberpunk2077/mods/16349/files/85356?key=6SS...d4cA&expires=1724532165&user_id=14184246
    // for: https://www.nexusmods.com/cyberpunk2077/mods/16349?tab=files
    
    static UriHandler()
    {
        // TODO: added logging.
        _logger = null;
    }

    public static void AddUriSchemeHandler()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            HandleLinuxUri();
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            HandleWindowsUri();
            return;
        }
        
        return;
    }

    private static void HandleWindowsUri()
    {
        throw new NotImplementedException();
    }

    private static void HandleLinuxUri()
    {
        throw new NotImplementedException();
    }
}