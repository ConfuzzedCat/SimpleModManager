using System.Diagnostics;
using SimpleModManager.Model;

namespace SimpleModManager;

public class GameHandler
{
    public GameHandler(GameModSettings? modSettings)
    {
        ModSettings = modSettings ?? throw new ArgumentNullException(nameof(modSettings));
    }

    public GameModSettings ModSettings { get; private set; }

    //TODO: Implement this class

    public void OpenGame()
    {
        var url = $"steam://rungameid/{ModSettings.SteamId}";
        var psi = new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = url
        };
        Process.Start(psi);
    }

    public void ShowArchiveMods()
    {
        
    }
}