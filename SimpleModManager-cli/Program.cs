using SimpleModManager;
using SimpleModManager.Model;
using SimpleModManager_cli.Utils;
using SimpleModManager.Util;

namespace SimpleModManager_cli;

internal class Program
{
    private static int Main(string[] args)
    {
        CliClientIo.Init();
        var logger = LoggerHandler.GetLogger<Program>();
        try
        {
            //UriHandler.AddUriSchemeHandler();
            var parsedArguments = CommandLineParser.ParseArguments(args);
            switch (parsedArguments.Count)
            {
                case 1:
                    // Print help to cli

                    if (parsedArguments.ContainsKey("-h") || parsedArguments.ContainsKey("--help"))
                    {
                        Console.WriteLine("WIP");
                    }

                    return 0;

                case 0:
                    // Get game
                    // Set it to gameName
                    var gameId = ModManager.ClientIo.Read(
                        "What game do want to mod? Needs to be it's NexusMods id eg. cyberpunk2077");
                    Console.Clear();
                    ModManager.ModGame(gameId);

                    ModLoop();

                    break;

                case 2:
                    // setup paths
                    // 
                    Console.WriteLine("WIP");
                    return 0;
                //break;
            }
        }
        catch (Exception e)
        {
            logger.Fatal(e, "SMM Crashed!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("SMM Crashed!\n{0}", e);
            return 1;
        }
        finally
        {
            ModManager.Exit();
        }

        return 0;
    }

    private static void ModLoop()
    {
        var prompt =
            $"{ConsoleUtil.BoldUnderlineFirstChar("Manage mods")} - " +
            $"{ConsoleUtil.BoldUnderlineFirstChar("Install mods")} - " +
            $"install mods from {ConsoleUtil.BoldUnderlineFirstChar("Path")} - " +
            $"change {ConsoleUtil.BoldUnderlineFirstChar("Settings")} - " +
            $"{ConsoleUtil.BoldUnderlineFirstChar("Exit")}";

        var doExit = false;
        do
        {
            var cmd = ModManager.ClientIo.ReadChar(
                prompt);
            Console.Clear();
            switch (cmd)
            {
                case 'M':
                case 'm':
                    // show installed mods
                    ControlMods();
                    break;
                case 'I':
                case 'i':
                    var filepath =
                        ModManager.ClientIo.Read("Paste in the file path of the mod. (.7z/.rar/.zip)");
                    if (string.IsNullOrEmpty(filepath))
                    {
                        break;
                    }
                    ModManager.InstallMod(filepath);
                    break;
                
                case 'P':
                case 'p':
                    var folderpath =
                        ModManager.ClientIo.Read("Paste in the folder containing the mods");
                    if (string.IsNullOrEmpty(folderpath) || !Directory.Exists(folderpath))
                    {
                        break;
                    }
                    ModManager.InstallMods(folderpath);
                    break;
                
                case 'S':
                case 's':
                    ChangeSettings();
                    break;
                case 'E':
                case 'e':
                    doExit = true;
                    break;
            }

            Console.Clear();
        } while (!doExit);
    }

    private static void ChangeSettings()
    {
        string archiveDirPrompt = $"{ConsoleUtil.BoldUnderlineFirstChar("Archive Directory (path)")}";
        string stagingDirPrompt = $"{ConsoleUtil.BoldUnderlineFirstChar("Staging Directory (path)")}";
        string rememberOverwriteChoosePrompt = $"{ConsoleUtil.BoldUnderlineFirstChar("Remember Overwrite Choose (boolean)")}";
        
        while (true)
        {
            Console.Clear();
            ModManager.ClientIo.Write(archiveDirPrompt);
            ModManager.ClientIo.Write(stagingDirPrompt);
            ModManager.ClientIo.Write(rememberOverwriteChoosePrompt);
            var choose = ModManager.ClientIo.ReadChar($"Which setting do you want to change? {ConsoleUtil.BoldUnderlineFirstChar("Exit")}");
            switch (choose)
            {
                case 'A':
                case 'a':
                    Console.Clear();
                    ModManager.ClientIo.Write($"Current: {SettingsManager.Settings.ArchiveDir}");
                    var newPathArchive = ModManager.ClientIo.Read("Type the new path for the archive directory. Path should have '{game}'");
                    SettingsManager.Settings.ArchiveDir = newPathArchive;
                    break;
                
                case 'S':
                case 's':
                    Console.Clear();
                    ModManager.ClientIo.Write($"Current: {SettingsManager.Settings.StagingDir}");
                    var newPathStaging = ModManager.ClientIo.Read("Type the new path for the archive directory. Path should have '{game}'");
                    SettingsManager.Settings.StagingDir = newPathStaging;
                    break;
                
                case 'R':
                case 'r':
                    Console.Clear();
                    ModManager.ClientIo.Write($"Current: {SettingsManager.Settings.RememberOverwriteChoose}");
                    var newOverwriteBool = ModManager.ClientIo.ReadBool("Type the new value", false);
                    SettingsManager.Settings.RememberOverwriteChoose = newOverwriteBool;
                    break;
                case 'E':
                case 'e':
                    return;
            }
        }
    }

    private static void ControlMods()
    {
        while (true)
        {
            var allMods = ModManager.CurrentGame.Mods.GetAllMods();
            for (var i = 0; i < allMods.Count; i++)
            {
                var mod = allMods[i];
                var s = $"{i + 1}. [{(mod.Installed ? "x" : " ")}] {mod.Name} (id: {mod.Id} - v: {mod.Version})";
                ModManager.ClientIo!.Write(s);
            }

            string input =
                ModManager.ClientIo!.Read(
                    "Type type the number of the mod, you want to (un)install or 'exit' to stop.");
            // input: d
            // if "d*" set doDelete = true
            bool doDelete = false;
            if (input.StartsWith('d') || input.StartsWith('D'))
            {
                doDelete = true;
                input = input[1..];
            }
            if (int.TryParse(input, out int num))
            {
                if (num > allMods.Count || num < 1)
                {
                    Console.Clear();
                    ModManager.ClientIo.Write("Number was out of range.");
                    continue;
                }

                num--;
                Billy(allMods, num, doDelete);
                continue;
            }

            // input: 1-10
            // input: 1,2,3,4,6,10
            var range = input.Split('-');
            if (range.Length == 2)
            {
                if (int.TryParse(range[0], out var startRange) && int.TryParse(range[1], out var endRange))
                {
                    if (startRange > allMods.Count || startRange < 1 || endRange > allMods.Count || endRange < 1)
                    {
                        Console.Clear();
                        ModManager.ClientIo.Write("Numbers was out of range.");
                        continue;
                    }

                    for (int i = startRange - 1; i < endRange; i++)
                    {
                        Billy(allMods, i, doDelete);
                    }

                    continue;
                }

                Console.Clear();
                //TODO: Change message.
                ModManager.ClientIo.Write("you dumb dumb format: num1-num2");
                continue;
            }

            break;
        }
    }

    private static void Billy(List<Mod> allMods, int num, bool doDelete)
    {
        var mod = allMods[num];
        if (doDelete)
        {
            ModManager.DeleteMod(mod);
            Console.Clear();
            return;
        }
        switch (mod.Installed)
        {
            case true:
                ModManager.UninstallMod(mod);
                break;
            case false:
                ModManager.InstallMod(mod);
                break;
        }

        Console.Clear();
    }
}