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
        var read = ModManager.ClientIo.Read("Test");
        Console.WriteLine(read);
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