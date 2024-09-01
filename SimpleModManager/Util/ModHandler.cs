using System.Text.RegularExpressions;
using SimpleModManager.Api;
using SimpleModManager.Model;

namespace SimpleModManager.Util;

public partial class ModHandler
{
    public static Mod FromFile(string filePath)
    {
        if (ModManager.CurrentGame == null) throw new Exception("CurrentGame is null. SMM has been setup incorrectly.");

        ModManager.ClientIo.Write($"Installing: {filePath}");

        var gameId = ModManager.CurrentGame.ModSettings.Id;
        var modId = ParseModId(filePath);
        var isInstalled = false;
        ModInfoApi apiInfo = ModInfoApi.Empty;
        if (!string.IsNullOrEmpty(ModManager.Apikey))
        {
            apiInfo = new ApiClient().RequestModInfo(gameId, modId).GetAwaiter().GetResult();
        }

        var version = ParseVersion(filePath, apiInfo.version);
        var fileName = ParseFileName(filePath);

        // unzip and register files

        var currentStagingFolder = ModManager.GetCurrentStagingFolder();
        var extractDir = Path.Combine(currentStagingFolder, $"{modId}##{fileName}##{version}");
        ExtractorHandler.ExtractFromFile(filePath, extractDir);
        
        // Handle weird mod structures
        // TODO: make more robust and handle more cases 
        var directories = Directory.GetDirectories(extractDir);
        switch (directories.Length)
        {
            case 0:
                // check files
                HandleLooseFiles(extractDir);
                break;
            case 1:
                // Check weird dir structure
                HandleWeirdFolderStructure(directories[0]);
                break;
        }

        var mod = new Mod(modId, gameId, fileName, isInstalled, extractDir, version,
            VFSHandler.CreateFromPath(extractDir));
        return mod;
    }

    private static void HandleWeirdFolderStructure(string directory)
    {
        var dirInfo = new DirectoryInfo(directory);
        if (ModManager.CurrentGame.ModSettings.ModStructures.Any(modStructure => modStructure.ModPath.StartsWith(dirInfo.Name)))
        {
            return;
        }

        foreach (var directoryInfo in dirInfo.GetDirectories())
        {
            Directory.Move(directoryInfo.FullName,dirInfo.Parent!.FullName);
        }
        Directory.Delete(dirInfo.FullName);
    }

    private static void HandleLooseFiles(string extractDir)
    {
        var files = Directory.GetFiles(extractDir);
        if (files.Length < 1)
        {
            throw new Exception("Mod archive was empty.");
        }

        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            foreach (var modStructure in ModManager.CurrentGame.ModSettings.ModStructures)
            {
                if (!modStructure.FileExtensions.Contains(fileInfo.Extension))
                {
                    continue;
                }

                var newPath = Path.Combine(extractDir, modStructure.ModPath);
                Directory.CreateDirectory(newPath);
                File.Move(file, Path.Combine(newPath, fileInfo.Name));
                break;
            }
        }
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
        {
            if (latestVersion == null)
            {
                throw new Exception("Couldn't parse Version, and Latest version was null.");
            }

            return latestVersion;
        }

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