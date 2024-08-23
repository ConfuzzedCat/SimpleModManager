using SimpleModManager;
using SimpleModManager_cli.Utils;

namespace SimpleModManager_cli;

internal class Program
{
    private static int Main(string[] args)
    {
        var logger = LoggerHandler.GetLogger<Program>();
        try
        {
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