using System.Diagnostics;
using SimpleModManager.Model;

namespace SimpleModManager;

public class GameHandler
{
    public GameModSettings ModSettings { get; private set; }

    //TODO: Implement this class

    public void OpenGame()
    {
        //var url = $"steam://rungameid/{ModSettings.SteamId}";
        var url = $"steam://rungameid/1091500";
        var psi = new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = url
        };
        Process.Start(psi);
    }
}