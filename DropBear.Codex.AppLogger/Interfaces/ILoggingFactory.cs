using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Interfaces;

/// <summary>
///     Defines the interface for a logging factory.
/// </summary>
public interface ILoggingFactory
{
    /// <summary>
    ///     Creates and returns a logger instance.
    /// </summary>
    /// <typeparam name="T">The type for which the logger is being created.</typeparam>
    /// <returns>The logger instance.</returns>
    ILogger CreateLogger<T>();
}
