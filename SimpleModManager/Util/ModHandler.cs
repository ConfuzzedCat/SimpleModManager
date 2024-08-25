using System.Text.RegularExpressions;
using SimpleModManager.Api;
using SimpleModManager.Model;

namespace SimpleModManager.Util;

public partial class ModHandler
{
    //TODO: finish implementing this method.
    public static Mod FromFile(string filePath)
    {
        if (ModManager.CurrentGame == null) throw new Exception("CurrentGame is null. SMM has been setup incorrectly.");

        ModManager.ClientIo.Write($"Installing: {filePath}");
        var gameId = ModManager.CurrentGame.ModSettings.Id;
        var modId = ParseModId(filePath);
        var isInstalled = false;
        var apiInfo = new ApiClient().RequestModInfo(gameId, modId).GetAwaiter().GetResult();
        var version = RequestVersion(apiInfo.version);


        return null;
    }

    private static int ParseModId(string filePath)
    {
        var regex = ModIdRegex();

        var match = regex.Match(filePath);
        if (match.Success && int.TryParse(match.Groups[1].Value, out var modId)) return modId;

        string input;
        do
        {
            input = ModManager.ClientIo.Read("Mod Id couldn't parsed from file. Manually type id");
        } while (int.TryParse(input, out modId));

        return modId;
    }

    private static string RequestVersion(string latestVersion)
    {
        var input = ModManager.ClientIo.Read("Type version of mod. type 'latest' or nothing for latest. eg. 1.2.2");
        if (string.IsNullOrEmpty(input) || input.Equals("latest", StringComparison.InvariantCultureIgnoreCase))
            return latestVersion;

        return input;
    }

    [GeneratedRegex(@"-(\d+)-")]
    private static partial Regex ModIdRegex();
}