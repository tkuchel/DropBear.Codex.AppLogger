using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Interfaces;

/// <summary>
///     Interface for defining logging strategies.
/// </summary>
public interface ILoggingStrategy
{
    /// <summary>
    ///     Logs a debug message.
    /// </summary>
    /// <param name="message">The debug message to log.</param>
    void LogDebug(string message);

    /// <summary>
    ///     Logs an informational message.
    /// </summary>
    /// <param name="message">The informational message to log.</param>
    void LogInformation(string message);

    /// <summary>
    ///     Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    void LogWarning(string message);

    /// <summary>
    ///     Logs an error message along with an optional exception.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="exception">The exception associated with the error, if any.</param>
    void LogError(string message, Exception? exception = null);

    /// <summary>
    ///     Logs a critical error message along with an optional exception.
    /// </summary>
    /// <param name="message">The critical error message to log.</param>
    /// <param name="exception">The exception associated with the critical error, if any.</param>
    void LogCritical(string message, Exception? exception = null);

    /// <summary>
    /// Gets the logger instance for the specified type.
    /// </summary>
    /// <typeparam name="TContext">The type of the logger to get.</typeparam>
    /// <returns>The logger instance for the specified type.</returns>
    ILogger<TContext> GetLogger<TContext>();
}
