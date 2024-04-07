
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Interfaces;

/// <summary>
///     Defines the interface for a logging factory.
/// </summary>
public interface ILoggerFactory
{
    /// <summary>
    ///     Creates and returns a logger instance for the specified type.
    /// </summary>
    /// <typeparam name="T">The type for which the logger is being created.</typeparam>
    /// <returns>The logger instance.</returns>
    ILogger<T> CreateLogger<T>();

    /// <summary>
    ///     Creates and returns a logger instance with the specified category name.
    /// </summary>
    /// <param name="categoryName">The category name for the logger.</param>
    /// <returns>The logger instance.</returns>
    ILogger CreateLogger(string categoryName);
}
