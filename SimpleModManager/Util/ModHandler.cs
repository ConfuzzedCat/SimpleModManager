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
        var version = ParseVersion(filePath, apiInfo.version);
        var fileName = ParseFileName(filePath);

        // unzip and register files

        var currentStagingFolder = ModManager.GetCurrentStagingFolder();
        var extractDir = Path.Combine(currentStagingFolder, $"{modId}##{fileName}##{version}");
        ExtractorHandler.ExtractFromFile(filePath, extractDir);

        var mod = new Mod(modId, gameId, fileName, isInstalled, extractDir, version,
            VFSHandler.CreateFromPath(extractDir));


        return mod;
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

    private static string ParseVersion(string filePath, string latestVersion)
    {
        var regex = ModVersionRegex();

        var match = regex.Match(filePath);
        if (match.Success)
        {
            return match.Groups[1].Value.Replace('-', '.');
        }

        var input = ModManager.ClientIo.Read(
            "Couldn't parse version from file. Please type the version or 'Latest' for latest found");
        if (string.IsNullOrEmpty(input) || input.Equals("latest", StringComparison.InvariantCultureIgnoreCase))
            return latestVersion;

        return input;
    }

    private static string ParseFileName(string filePath)
    {
        var regex = ModFileNameRegex();

        var match = regex.Match(filePath);
        if (match.Success)
        {
            return match.Groups[1].Value[(match.Groups[1].Value.LastIndexOf(Path.DirectorySeparatorChar) + 1)..];
        }

        // TODO: Change input prompt
        var input = ModManager.ClientIo.Read(
            "Couldn't parse Filename, eg. <filename>-<modid>-<version>-<filehash>.zip. Please type the name");
        return input;
    }

    [GeneratedRegex(@"-(\d+)-")]
    private static partial Regex ModIdRegex();

    [GeneratedRegex(@"-\d+-([\w-]+)-")]
    private static partial Regex ModVersionRegex();

    [GeneratedRegex(@"^(.*?)-\d+-")]
    private static partial Regex ModFileNameRegex();
}