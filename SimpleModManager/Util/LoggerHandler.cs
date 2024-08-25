using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace SimpleModManager.Util;

public static class LoggerHandler
{
    private static Logger? _logger;

    private static Logger CreateLogger()
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Is(LogEventLevel.Debug)
            .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}][{SourceContext}] - {Message}{NewLine}{Exception}")
            .CreateLogger();
        return _logger;
    }

    public static ILogger GetLogger<T>()
    {
        return _logger == null ? CreateLogger().ForContext<T>() : _logger.ForContext<T>();
    }
}