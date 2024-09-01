using System.Collections;
using SimpleModManager;
using SimpleModManager_cli.Utils;
using SimpleModManager.Util;

namespace SimpleModManager_cli;

public sealed class CliClientIo : AClientIo
{
    private CliClientIo() : base(LoggerHandler.GetLogger<CliClientIo>())
    {
    }

    public static void Init()
    {
        ModManager.ClientIo = new CliClientIo();
    }

    public override string Read(string prompt)
    {
        Console.Write(prompt + ": ");
        var readLine = Console.ReadLine();
        if (readLine is null) throw new Exception("Input was null");
        Logger.Debug("Asked client for input \"{0}\". Answer: {1}", prompt, readLine);
        return readLine;
    }

    public override void Write(string input)
    {
        Logger.Debug("Wrote to client \"{0}\"", input);
        Console.WriteLine(input);
    }

    public override void Show<T>(T data)
    {
        if (IsEnumerableType(typeof(T)))
        {
            foreach (var d in (data as IEnumerable)!)
            {
                Console.WriteLine(d);
            }
            return;
        }

        Console.WriteLine(data);
    }

    public override bool ReadBool(string prompt, bool defaultValue)
    {
        string addon;

        if (defaultValue)
        {
            //addon = "(TRUE/false)"
            addon = "(" + ConsoleUtil.Underline("TRUE") + "/false)";
        }
        else
        {
            addon = "(true/"+ConsoleUtil.Underline("FALSE")+")";
        }
        Console.Write("{0} {1}: ", prompt, addon);
        var readLine = Console.ReadLine();
        if (readLine is null) throw new Exception("Input was null");
        Logger.Debug("Asked client for input \"{0}\". Answer: {1}", prompt, readLine);

        return string.IsNullOrEmpty(readLine) ? defaultValue : Convert.ToBoolean(readLine);
    }

    public override char ReadChar(string prompt)
    {
        Console.Write(prompt + ": ");
        var readLine = Console.ReadKey().KeyChar;
        Logger.Debug("Asked client for input \"{0}\". Answer: {1}", prompt, readLine);
        return readLine;
    }

    private static bool IsEnumerableType(Type type)
    {
        return type.Name != nameof(String) 
               && type.GetInterface(nameof(IEnumerable)) != null;
    }
}