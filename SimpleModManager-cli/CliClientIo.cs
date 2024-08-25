using Serilog;
using SimpleModManager;
using SimpleModManager.Util;

namespace SimpleModManager_cli;

public sealed class CliClientIo : AClientIo
{
    public static void Init()
    {
        ModManager.ClientIo = new CliClientIo();
    }
    
    
    private CliClientIo() : base(LoggerHandler.GetLogger<CliClientIo>())
    { }
    
    public override string Read(string prompt)
    {
        Console.Write(prompt + ": ");
        var readLine = Console.ReadLine();
        if (readLine is null)
        {
            throw new Exception("Input was null");
        }
        Logger.Debug("Asked client for input \"{0}\". Answer: {1}", prompt, readLine);
        return readLine;
    }

    public override void Write(string input)
    {
        Logger.Debug("Wrote to client \"{0}\"", input);
        Console.WriteLine(input);
    }
}