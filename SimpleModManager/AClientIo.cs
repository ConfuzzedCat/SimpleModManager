using Serilog;

namespace SimpleModManager;

public abstract class AClientIo
{
    protected readonly ILogger Logger;

    protected AClientIo(ILogger logger)
    {
        Logger = logger;
    }

    public abstract string Read(string prompt);
    
    public abstract void Write(string input);

}