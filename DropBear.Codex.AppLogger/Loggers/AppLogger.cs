using DropBear.Codex.AppLogger.Interfaces;
using DropBear.Codex.AppLogger.LoggingStrategies;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Loggers;

/// <summary>
///     Provides logging functionality for the specified type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type for which logging is provided.</typeparam>
public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILoggingStrategy _loggingStrategy;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppLogger{T}" /> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public AppLogger(ILogger<T> logger)
    {
        // Determine the appropriate logging strategy based on the type of logger.
        if (logger.GetType().Name.Contains("ZLogger", StringComparison.OrdinalIgnoreCase))
            _loggingStrategy = new ZLoggerStrategy<T>(logger);
        else
            _loggingStrategy = new StandardLoggerStrategy<T>(logger);
    }

    /// <inheritdoc />
    public void LogDebug(string message) => _loggingStrategy.LogDebug(message);

    /// <inheritdoc />
    public void LogInformation(string message) => _loggingStrategy.LogInformation(message);

    /// <inheritdoc />
    public void LogWarning(string message) => _loggingStrategy.LogWarning(message);

    /// <inheritdoc />
    public void LogError(string message) => _loggingStrategy.LogError(message);

    /// <inheritdoc />
    public void LogError(Exception exception, string message = "") => _loggingStrategy.LogError(message, exception);

    /// <inheritdoc />
    public void LogCritical(string message) => _loggingStrategy.LogCritical(message);

    /// <inheritdoc />
    public void LogCritical(Exception exception, string message = "") =>
        _loggingStrategy.LogCritical(message, exception);
}
