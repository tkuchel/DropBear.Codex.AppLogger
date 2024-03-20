using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace DropBear.Codex.AppLogger.LoggingStrategies;

public class ZLoggerStrategy<T>(ILogger<T> logger) : ILoggingStrategy
{
    public void LogDebug(string message) => logger.ZLogDebug($"Processed: {message}");
    public void LogInformation(string message) => logger.ZLogInformation($"Processed: {message}");
    public void LogWarning(string message) => logger.ZLogWarning($"Processed: {message}");

    public void LogError(string message, Exception? exception = null) =>
        logger.ZLogError(exception, $"Processed: {message}");

    public void LogCritical(string message, Exception? exception = null) =>
        logger.ZLogCritical(exception, $"Processed: {message}");
}
