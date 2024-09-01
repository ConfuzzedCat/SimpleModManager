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

    public abstract void Show<T>(T data);
    public abstract bool ReadBool(string prompt, bool defaultValue);
    public abstract char ReadChar(string prompt);
}