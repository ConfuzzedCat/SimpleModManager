using Serilog;

namespace SimpleModManager;

public class LoggerHandler
{
    private static ILogger? _logger;

    private static ILogger CreateLogger()
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File("log.txt", 
                rollingInterval: RollingInterval.Day, 
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}][{SourceContext}] - {Message}{NewLine}{Exception}")
            .CreateLogger();
        return _logger;
    }

    public static ILogger GetLogger<T>() => _logger == null ? CreateLogger().ForContext<T>() : _logger.ForContext<T>();
}