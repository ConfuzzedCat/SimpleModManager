using System.Text.Json;
using SimpleModManager;
using SimpleModManager_cli.Utils;
using SimpleModManager.Model;
using SimpleModManager.Util;
using Vogen;

namespace SimpleModManager_cli;

internal class Program
{
    private static int Main(string[] args)
    {
        //var _logger = LoggerHandler.GetLogger<Program>();
        //_logger.Information("Test");
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
            $"{ConsoleUtil.BoldUnderline("M")}anage mods - {ConsoleUtil.BoldUnderline("I")}nstall mods - {ConsoleUtil.BoldUnderline("E")}xit";

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
                case 'E':
                case 'e':
                    doExit = true;
                    break;
            }
            Console.Clear();
        } while (!doExit);
    }

    private static void ControlMods()
    {
        while (true)
        {
            var allMods = ModManager.CurrentGame.Mods.GetAllMods();
            for (var i = 0; i < allMods.Count; i++)
            {
                var mod = allMods[i];
                var s = $"{i + 1}. [{(mod.Installed ? "x" : " ")}] {mod.Name} (id: {mod.Id})";
                ModManager.ClientIo!.Write(s);
            }

            string input = ModManager.ClientIo!.Read("Type type the number of the mod, you want to (un)install or 'exit' to stop.");
            if (int.TryParse(input, out int num))
            {
                if (num > allMods.Count || num < 1)
                {
                    Console.Clear();
                    ModManager.ClientIo.Write("Number was out of range.");
                    continue;
                }

                num--;
                var mod = allMods[num];
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
                continue;
            }
            break;
        }
    }
}