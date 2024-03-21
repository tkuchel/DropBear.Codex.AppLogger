using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace DropBear.Codex.AppLogger.LoggingStrategies;

/// <summary>
///     Provides a logging strategy using ZLogger.
/// </summary>
/// <typeparam name="T">The type of logger.</typeparam>
public class ZLoggerStrategy<T> : ILoggingStrategy
{
    private readonly ILogger<T> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ZLoggerStrategy{T}" /> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public ZLoggerStrategy(ILogger<T> logger) => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public void LogDebug(string message) => _logger.ZLogDebug($"[D]: {message}");

    /// <inheritdoc />
    public void LogInformation(string message) => _logger.ZLogInformation($"[I]: {message}");

    /// <inheritdoc />
    public void LogWarning(string message) => _logger.ZLogWarning($"[W]: {message}");

    /// <inheritdoc />
    public void LogError(string message, Exception? exception = null) =>
        _logger.ZLogError(exception, $"[E]: {message}");

    /// <inheritdoc />
    public void LogCritical(string message, Exception? exception = null) =>
        _logger.ZLogCritical(exception, $"[C]: {message}");
}
