using System.Diagnostics;
using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using NexusMods.Paths;
using Serilog;
using SimpleModManager.Model;

namespace SimpleModManager.Util;

public class GameHandler
{
    public GameHandler(GameModSettings? modSettings)
    {
        _logger = LoggerHandler.GetLogger<GameHandler>();
        ModSettings = modSettings ?? throw new ArgumentNullException(nameof(modSettings));
        GamePath = FindGame();
    }

    private readonly ILogger _logger;
    public GameModSettings ModSettings { get; }

    public String GamePath { get; set; }

    private string FindGame()
    {
        var handler = new SteamHandler(FileSystem.Shared, OperatingSystem.IsWindows() ? WindowsRegistry.Shared : null);
        var appid = AppId.From(uint.Parse(ModSettings.SteamId));
        var game = handler.FindOneGameById(appid, out var errors);
        foreach (var error in errors)
        {
            _logger.Error("Error trying to find game. {0}", error);
        }

        if (game is null)
        {
            throw new Exception($"[Steam] Couldn't find game: {ModSettings.Id}");
        }

        return game.Path.GetFullPath();
    }

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