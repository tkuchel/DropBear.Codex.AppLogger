using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.LoggingStrategies;

/// <summary>
///     Provides a standard logging strategy using <see cref="ILogger{T}" />.
/// </summary>
/// <typeparam name="T">The type of logger.</typeparam>
public class StandardLoggerStrategy<T> : ILoggingStrategy
{
    private readonly ILogger<T> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StandardLoggerStrategy{T}" /> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public StandardLoggerStrategy(ILogger<T> logger) =>
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public void LogDebug(string message)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1, "LogDebug"), "{Message}");
        logAction(_logger, message, null);
    }

    /// <inheritdoc />
    public void LogInformation(string message)
    {
        var logAction =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(2, "LogInformation"), "{Message}");
        logAction(_logger, message, null);
    }

    /// <inheritdoc />
    public void LogWarning(string message)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Warning, new EventId(3, "LogWarning"), "{Message}");
        logAction(_logger, message, null);
    }

    /// <inheritdoc />
    public void LogError(string message, Exception? exception = null)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Error, new EventId(4, "LogError"), "{Message}");
        logAction(_logger, message, exception);
    }

    /// <inheritdoc />
    public void LogCritical(string message, Exception? exception = null)
    {
        var logAction = LoggerMessage.Define<string>(LogLevel.Critical, new EventId(5, "LogCritical"), "{Message}");
        logAction(_logger, message, exception);
    }
}
