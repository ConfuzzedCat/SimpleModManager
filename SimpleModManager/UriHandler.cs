using System.Runtime.InteropServices;
using Serilog;

namespace SimpleModManager;

public static class UriHandler
{
    private static ILogger _logger; 
    
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