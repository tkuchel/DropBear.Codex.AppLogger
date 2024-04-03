using DropBear.Codex.AppLogger.Interfaces;
using DropBear.Codex.AppLogger.LoggingStrategies;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Loggers;

/// <summary>
/// Provides logging functionality for the specified type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type for which logging is provided.</typeparam>
public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILoggingStrategy _loggingStrategy;
    private readonly ILoggerFactory _loggerFactory;

    private const string ZLoggerTypeName = "ZLogger";

    /// <summary>
    /// Initializes a new instance of the <see cref="AppLogger{T}" /> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="loggerFactory">The logger factory instance.</param>
    public AppLogger(ILogger<T> logger, ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        // Determine the appropriate logging strategy based on the type of logger.
        Type loggerType = logger.GetType();
        _loggingStrategy = (loggerType.Name.Contains(ZLoggerTypeName, StringComparison.OrdinalIgnoreCase)
            ? (ILoggingStrategy)Activator.CreateInstance(typeof(ZLoggerStrategy<>).MakeGenericType(typeof(T)), logger, loggerFactory)!
            : new StandardLoggerStrategy<T>(logger, loggerFactory)) ?? throw new InvalidOperationException("Unable to determine the appropriate logging strategy.");
    }

    /// <inheritdoc />
    public void LogDebug(string message) => _loggingStrategy.LogDebug(message);

    /// <inheritdoc />
    public void LogInformation(string message) => _loggingStrategy.LogInformation(message);

    /// <inheritdoc />
    public void LogWarning(string message) => _loggingStrategy.LogWarning(message);

    /// <inheritdoc />
    public void LogError(string message) => _loggingStrategy.LogError(message, null);

    /// <inheritdoc />
    public void LogError(Exception exception, string message = "") => _loggingStrategy.LogError(message, exception);

    /// <inheritdoc />
    public void LogCritical(string message) => _loggingStrategy.LogCritical(message, null);

    /// <inheritdoc />
    public void LogCritical(Exception exception, string message = "") => _loggingStrategy.LogCritical(message, exception);

    /// <inheritdoc />
    public IAppLogger<TContext> ForContext<TContext>()
    {
        ILogger<TContext> logger = _loggingStrategy.GetLogger<TContext>();
        return new AppLogger<TContext>(logger, _loggerFactory);
    }
}