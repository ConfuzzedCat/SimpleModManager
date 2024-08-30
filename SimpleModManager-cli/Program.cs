using System.Text.Json;
using SimpleModManager;
using SimpleModManager_cli.Utils;
using SimpleModManager.Util;

namespace SimpleModManager_cli;

internal class Program
{
    private static int Main(string[] args)
    {
        //var _logger = LoggerHandler.GetLogger<Program>();
        //_logger.Information("Test");
        CliClientIo.Init();

        var testzip = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Black Lily-16363-1-0-1724410699.rar");
        //ExtractorHandler.ExtractFromFile(testzip, testzip.Remove(testzip.Length-4));

        ModManager.ModGame("cyberpunk2077");

        var testMod = ModHandler.FromFile(testzip);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var serialized = JsonSerializer.Serialize(testMod, jsonSerializerOptions);


        Console.WriteLine("\n\n\n");

        Console.WriteLine(serialized);
        File.WriteAllText(
            "/home/confuzzedcat/source/repos/SimpleModManager/SimpleModManager-cli/bin/Debug/net8.0/test.json",
            serialized);

        Console.WriteLine("\n\n\n");

        Console.WriteLine(testMod.ModFiles.Display());
        Console.ReadKey();

        //var client = new ApiClient();
        //Console.WriteLine(client.RequestModInfo("cyberpunk2077", 16363).GetAwaiter().GetResult());
        //_logger.Information("Got Result.");
        //Console.ReadKey();

        return 0;
        var logger = LoggerHandler.GetLogger<Program>();
        try
        {
            UriHandler.AddUriSchemeHandler();
            var parsedArguments = CommandLineParser.ParseArguments(args);
            var gameName = string.Empty;
            switch (parsedArguments.Count)
            {
                case 1:
                    // Print help to cli
                    return 0;

                case 0:
                // Get game
                // Set it to gameName

                case 2:
                    // setup paths
                    // 
                    break;
            }
        }
        catch (Exception e)
        {
            logger.Fatal(e, "SMM Crashed!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("SMM Crashed!\n{0}", e);
            return 1;
        }

        return 0;
    }
}
