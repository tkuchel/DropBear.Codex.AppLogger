using DropBear.Codex.AppLogger.Interfaces;
using DropBear.Codex.AppLogger.LoggingStrategies;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Loggers;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILoggingStrategy _loggingStrategy;

    public AppLogger(ILogger<T> logger)
    {
        if (logger.GetType().Name.Contains("ZLogger", StringComparison.OrdinalIgnoreCase))
            _loggingStrategy = new ZLoggerStrategy<T>(logger);
        else
            _loggingStrategy = new StandardLoggerStrategy<T>(logger);
    }

    public void LogDebug(string message) => _loggingStrategy.LogDebug(message);
    public void LogInformation(string message) => _loggingStrategy.LogInformation(message);
    public void LogWarning(string message) => _loggingStrategy.LogWarning(message);
    public void LogError(string message) => _loggingStrategy.LogError(message);
    public void LogError(Exception exception, string message = "") => _loggingStrategy.LogError(message, exception);
    public void LogCritical(string message) => _loggingStrategy.LogCritical(message);

    public void LogCritical(Exception exception, string message = "") =>
        _loggingStrategy.LogCritical(message, exception);
}
