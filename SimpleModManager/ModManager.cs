using System.Text.Json;
using Serilog;
using SimpleModManager.Model;
using SimpleModManager.Util;

namespace SimpleModManager;

public class ModManager
{
    private static readonly ILogger Logger;
    public static readonly string Apikey;

    // TODO: CRITICAL: Sanitize inputs solution wide. 
    static ModManager()
    {
        Logger = LoggerHandler.GetLogger<ModManager>();
        try
        {
            Apikey = LoadApikey();
        }
        catch (Exception e)
        {
            Logger.Warning("Can't load apikey, will try and guess the info. {e}", e);
            Apikey = string.Empty;
        }
    }

    public static GameHandler CurrentGame { get; private set; }
    //public static Node GameVFS { get; set; }
    //public static Node GameStagingVFS { get; set; }
    
    public static AClientIo? ClientIo { get; set; }

    public static void ModGame(string gameId)
    {
        try
        {
            var gamesModPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GameModSettings");
            if (!Directory.Exists(gamesModPath)) Directory.CreateDirectory(gamesModPath);

            var gameModSettingsFile = Path.Combine(gamesModPath, gameId + ".json");
            var gameStagedModsFile = Path.Combine(gamesModPath, gameId + "_mods.json");
            bool doesGameSettingsExist = File.Exists(gameModSettingsFile);
            if (!doesGameSettingsExist)
            {
                throw new FileNotFoundException($"Game Mod Settings file was not found for given id: {gameId}.");
            }

            Logger.Information("Game with id \"{0}\" found.", gameId);
            var content = File.ReadAllText(gameModSettingsFile);
            CurrentGame = new GameHandler(JsonSerializer.Deserialize<GameModSettings>(content));
            if (File.Exists(gameStagedModsFile))
            {
                var stagedModsContent = File.ReadAllText(gameStagedModsFile);
                var stagedMods = JsonSerializer.Deserialize<StagedMods>(stagedModsContent);
                if (stagedMods is not null)
                {
                    stagedMods.FetchMods();
                    CurrentGame.Mods = stagedMods;
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to load game mod settings. Probably invalid Json format.");
            string name = ClientIo.Read("What is the name of the game?");
            string id = ClientIo.Read("What is it nexus mod's game id of the game?");
            string partSteamId = ClientIo.Read("What is the steam id of the game?");
            string steamid = $"steam://rungameid/{partSteamId}";
            var gameSettings = new GameModSettings(id, name, steamid, [GameModSettings.ModFileSettingStructure.Default()]);
            CurrentGame = new GameHandler(gameSettings);
        }
        // TODO: integrate the vfs
        //GameVFS = VFSHandler.CreateFromPath(CurrentGame.GamePath);
    }

    public static void InstallMod(string filePath)
    {
        try
        {
            filePath = filePath.TrimEnd();
            if ((filePath.StartsWith('\'') && filePath.EndsWith('\'')) ||
                (filePath.StartsWith('"') && filePath.EndsWith('"')))
            {
                filePath = filePath[1..];
                filePath = filePath.Remove(filePath.Length - 1);
            }
            var mod = ModHandler.FromFile(filePath, out bool alreadyInstalled);
            if (!alreadyInstalled)
            {
                InstallMod(mod);
            }
        }
        catch (Exception e)
        {
            Logger.Warning(e, "An error happened when installing {0}. Skipping", filePath);
        }
    }
    public static void InstallMod(Mod mod)
    {
        mod.Install(CurrentGame.GamePath);
        CurrentGame.Mods.InstallMod(mod);
    }
    public static void UninstallMod(Mod mod)
    {
        mod.Uninstall(CurrentGame.GamePath);
        CurrentGame.Mods.StageMod(mod);
    }
    public static string GetCurrentStagingFolder()
    {
        return SettingsManager.Settings.StagingDir.Replace("{game}", CurrentGame.ModSettings.Id);
    }

    public static string GetCurrentArchiveFolder()
    {
        return SettingsManager.Settings.ArchiveDir.Replace("{game}", CurrentGame.ModSettings.Id);
    }


    private static string LoadApikey()
    {
        var dotEnvFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");

        if (!File.Exists(dotEnvFile))
        {
            File.WriteAllText(dotEnvFile, "apikey=\"\"");
            throw new FileNotFoundException(
                "File not found. Created the file. Remember to add your api into it (File is hidden). Just add the key from the link inside the quotes: https://next.nexusmods.com/settings/api-keys",
                ".env");
        }

        var fileContent = File.ReadAllLines(dotEnvFile)[0];
        var split = fileContent.Split('=');
        if (!split[0].StartsWith("apikey"))
            throw new Exception("Api key couldn't be found on the first line of .env file.");

        if (split.Length < 2) throw new Exception(".env has an invalid format");
        return fileContent[fileContent.IndexOf('"')..].Replace("\"", "");
    }

    public static void Exit()
    {
        var gamesModPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GameModSettings");
        var gameStagedModsFile = Path.Combine(gamesModPath, CurrentGame.ModSettings.Id + "_mods.json");
        var content = JsonSerializer.Serialize(CurrentGame.Mods);
        File.WriteAllText(gameStagedModsFile, content);
    }

    public static void InstallMods(string folderpath)
    {
        if (!Directory.Exists(folderpath))
        {
            throw new IOException("Directory doesn't exists.");
        }
        var filteredFiles = Directory
            .EnumerateFiles(folderpath) 
            .Where(file =>
            {
                //.7z/.rar/.zip
                return file.ToLower().EndsWith("7z") || 
                       file.ToLower().EndsWith("zip") || 
                       file.ToLower().EndsWith("rar");
            })
            .ToList();
        foreach (var file in filteredFiles)
        {
            InstallMod(file);
        }
    }

    public static void DeleteMod(Mod mod)
    {
        mod.Uninstall(CurrentGame.GamePath);
        mod.Delete();
        CurrentGame.Mods.DeleteMod(mod);
    }
}
