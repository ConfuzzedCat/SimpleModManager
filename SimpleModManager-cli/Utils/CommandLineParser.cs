namespace SimpleModManager_cli.Utils;

public static class CommandLineParser
{
    public static Dictionary<string, string> ParseArguments(string[] args)
    {
        if (args.Length == 0)
        {
            return new Dictionary<string, string>();
        }
        
        var parsedArgs = new Dictionary<string, string>();
        
        if (args.Length > 2)
        {
            throw new ArgumentException("Too many arguments provided");
        }
            
        var str = args[0];
        if (str.StartsWith("nxm://"))
        {
            //TODO: Handle nxm mod download
        }

        if (!str.StartsWith('-')) throw new ArgumentException($"Unexpected argument format: {str}");
        switch (str)
        {
            case "-h":
            case "--help":
                parsedArgs.Add(str, "");
                return parsedArgs;
            // Handle "-g" or "--game" option
            case "-g":
            case "--game":
            {
                if (args.Length != 2) throw new ArgumentException("Expected a value after '-g' or '--game'");
                var value = args[1];

                // Ensure there is only one argument for "-g" or "--game"
                if (value.StartsWith('-'))
                {
                    throw new ArgumentException("Expected a value after '-g' or '--game'");
                }

                parsedArgs.Add(str, value);
                return parsedArgs;
            }
            default:
                throw new ArgumentException($"Unknown argument: {str}");
        }
    }
}