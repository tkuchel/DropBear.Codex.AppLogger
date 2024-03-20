using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.LoggingStrategies;

public class StandardLoggerStrategy<T>(ILogger<T> logger) : ILoggingStrategy
{
    private readonly ILogger<T> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public void LogDebug(string message)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1, "LogDebug"), "{Message}");
        logAction(_logger, message, null);
    }

    public void LogInformation(string message)
    {
        var logAction =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(2, "LogInformation"), "{Message}");
        logAction(_logger, message, null);
    }

    public void LogWarning(string message)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(3, "LogWarning"), "{Message}");
        logAction(_logger, message, null);
    }

    public void LogError(string message, Exception? exception = null)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Error, new EventId(4, "LogError"), "{Message}");
        logAction(_logger, message, exception);
    }

    public void LogCritical(string message, Exception? exception = null)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Critical, new EventId(5, "LogCritical"), "{Message}");
        logAction(_logger, message, exception);
    }
}
