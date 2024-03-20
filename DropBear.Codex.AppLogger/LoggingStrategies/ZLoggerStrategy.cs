using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;
using ZLogger;
using System;

namespace DropBear.Codex.AppLogger.LoggingStrategies
{
    /// <summary>
    /// Provides a logging strategy using ZLogger.
    /// </summary>
    /// <typeparam name="T">The type of logger.</typeparam>
    public class ZLoggerStrategy<T> : ILoggingStrategy
    {
        private readonly ILogger<T> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZLoggerStrategy{T}"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public ZLoggerStrategy(ILogger<T> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public void LogDebug(string message) => _logger.ZLogDebug($"Processed: {message}");

        /// <inheritdoc/>
        public void LogInformation(string message) => _logger.ZLogInformation($"Processed: {message}");

        /// <inheritdoc/>
        public void LogWarning(string message) => _logger.ZLogWarning($"Processed: {message}");

        /// <inheritdoc/>
        public void LogError(string message, Exception? exception = null) =>
            _logger.ZLogError(exception, $"Processed: {message}");

        /// <inheritdoc/>
        public void LogCritical(string message, Exception? exception = null) =>
            _logger.ZLogCritical(exception, $"Processed: {message}");
    }
}
