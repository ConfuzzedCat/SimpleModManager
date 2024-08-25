using System.Text.RegularExpressions;
using SimpleModManager.Api;
using SimpleModManager.Model.VirtualFileSystem;

namespace SimpleModManager.Model;

public partial class Mod
{
    public required int Id { get; set; }
    public required string GameId { get; set; }
    public required string Name { get; set; }
    public bool Installed { get; set; }
    public string FilePath { get; set; }
    public string Version { get; set; } 
    public DirectoryNode ModFiles { get; set; }

    public Mod(int id, string gameId, string name, bool installed, string filePath, string version, DirectoryNode modFiles)
    {
        Id = id;
        GameId = gameId;
        Name = name;
        Installed = installed;
        FilePath = filePath;
        Version = version;
        ModFiles = modFiles;
    }

    //TODO: finish implementing this method.
    public static Mod FromFile(string filePath)
    {
        if (ModManager.CurrentGame == null)
        {
            throw new Exception("CurrentGame is null. SMM has been setup incorrectly.");
        }
        ModManager.ClientIo.Write($"Installing: {filePath}");
        var gameId = ModManager.CurrentGame.ModSettings.Id;
        var modId = ParseModId(filePath);
        var isInstalled = false;
        var apiInfo = new ApiClient().RequestModInfo(gameId, modId).GetAwaiter().GetResult();
        
        
        return null;
    }

    private static int ParseModId(string filePath)
    {

        var regex = ModIdRegex();

        var match = regex.Match(filePath);
        if (match.Success && int.TryParse(match.Groups[1].Value, out var modId))
        {
            return modId;
        }

        string input;
        do
        {
            input = ModManager.ClientIo.Read("Mod Id couldn't parsed from file. Manually type id");
        } while (int.TryParse(input, out modId));

        return modId;
    }

    private static string RequestVersion(string latestVersion)
    {
        var input = ModManager.ClientIo.Read("Type version of mod. type 'latest' for latest. eg. 1.2.2");
        //TODO: get
        return input;
    }

    [GeneratedRegex(@"-(\d+)-")]
    private static partial Regex ModIdRegex();
}