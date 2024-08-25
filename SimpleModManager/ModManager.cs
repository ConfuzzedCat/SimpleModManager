using Newtonsoft.Json;
using Serilog;
using SimpleModManager.Model;
using SimpleModManager.Model.VirtualFileSystem;
using SimpleModManager.Util;

namespace SimpleModManager;

public class ModManager
{
    private static readonly ILogger Logger;
    public static readonly string Apikey;

    static ModManager()
    {
        Logger = LoggerHandler.GetLogger<ModManager>();
        Apikey = LoadApikey();
    }

    public static GameHandler? CurrentGame { get; private set; }
    public static DirectoryNode GameVFS { get; set; }
    public static DirectoryNode GameStagingVFS { get; set; }
    
    public static AClientIo ClientIo { get; set; }

    public static void ModGame(string gameId)
    {
        var gamesModPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GameModSettings");
        if (!Directory.Exists(gamesModPath)) Directory.CreateDirectory(gamesModPath);

        var gameModSettingsFile = Path.Combine(gamesModPath, gameId + ".json");
        if (!File.Exists(gameModSettingsFile))
            throw new FileNotFoundException($"Game Mod Settings file was not found for given id: {gameId}.");
        Logger.Information("Game with id \"{0}\" found.", gameId);
        var content = File.ReadAllText(gameModSettingsFile);
        try
        {
            CurrentGame = new GameHandler(JsonConvert.DeserializeObject<GameModSettings>(content));
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to load game mod settings. Probably invalid Json format.");
        }

        GameVFS = VFSHandler.CreateFromPath(CurrentGame.GamePath);
    }

    public static string GetCurrentStagingFolder()
    {
        return CurrentGame is null
            ? SettingsManager.Settings.StagingDir
            : SettingsManager.Settings.StagingDir.Replace("{game}", CurrentGame.ModSettings.Id);
    }

    public static string GetCurrentArchiveFolder()
    {
        return CurrentGame is null
            ? SettingsManager.Settings.ArchiveDir
            : SettingsManager.Settings.ArchiveDir.Replace("{game}", CurrentGame.ModSettings.Id);
    }


    private static string LoadApikey()
    {
        var dotEnvFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");

        if (!File.Exists(dotEnvFile))
        {
            File.WriteAllText(dotEnvFile, "apikey=\"<key>\"");
            throw new FileNotFoundException(
                "File not found. Created the file. Remember to add your api into it (File is hidden). just replace '<key>' with your key from here: https://next.nexusmods.com/settings/api-keys",
                ".env");
        }

        var fileContent = File.ReadAllLines(dotEnvFile)[0];
        var split = fileContent.Split('=');
        if (!split[0].StartsWith("apikey"))
            throw new Exception("Api key couldn't be found on the first line of .env file.");

        if (split.Length < 2) throw new Exception(".env has an invalid format");
        return fileContent[fileContent.IndexOf('"')..].Replace("\"", "");
    }
}