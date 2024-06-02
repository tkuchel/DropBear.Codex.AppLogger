using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Providers;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

namespace DropBear.Codex.AppLogger.LoggingFactories;

/// <summary>
///     Factory for creating ZLogger instances.
/// </summary>
public sealed class ZLoggerFactory : ILoggerFactory, IDisposable
{
    private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;
    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ZLoggerFactory" /> class.
    /// </summary>
    /// <param name="logLevel">The minimum log level.</param>
    /// <param name="consoleOutput">Whether to enable console output.</param>
    /// <param name="rollingFilePath">The path for rolling file logs.</param>
    /// <param name="rollingSizeKB">The rolling size in KB.</param>
    /// <param name="useJsonFormatter">Whether to use JSON formatter.</param>
    public ZLoggerFactory(LogLevel logLevel, bool consoleOutput, string rollingFilePath, int rollingSizeKB,
        bool useJsonFormatter)
    {
        if (string.IsNullOrEmpty(rollingFilePath))
            throw new ArgumentException("Rolling file path cannot be empty", nameof(rollingFilePath));

        if (!Directory.Exists(rollingFilePath))
            throw new DirectoryNotFoundException($"Directory {rollingFilePath} does not exist");

        _loggerFactory = LoggerFactory.Create(builder =>
        {
            ConfigureZLogger(builder, logLevel, consoleOutput, rollingFilePath, rollingSizeKB, useJsonFormatter);
        });
    }

    /// <summary>
    ///     Disposes the factory.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Creates a logger instance for the specified type.
    /// </summary>
    /// <typeparam name="T">The type for which the logger is being created.</typeparam>
    /// <returns>The logger instance.</returns>
    public ILogger<T> CreateLogger<T>()
    {
        ThrowIfDisposed();
        return _loggerFactory.CreateLogger<T>();
    }

    /// <summary>
    ///     Creates a logger instance with the specified category name.
    /// </summary>
    /// <param name="categoryName">The category name for the logger.</param>
    /// <returns>The logger instance.</returns>
    public ILogger? CreateLogger(string categoryName)
    {
        ThrowIfDisposed();
        return _loggerFactory.CreateLogger(categoryName);
    }

    /// <summary>
    ///     Configures the logger factory.
    /// </summary>
    /// <param name="configurationAction">The configuration action.</param>
    public static void Configure(Action configurationAction) => configurationAction?.Invoke();

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) _loggerFactory.Dispose();
        _disposed = true;
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, GetType().FullName!);

    private static void ConfigureZLogger(ILoggingBuilder builder, LogLevel logLevel, bool consoleOutput,
        string rollingFilePath, int rollingSizeKB, bool useJsonFormatter)
    {
        builder.ClearProviders()
            .SetMinimumLevel(logLevel);

        if (consoleOutput)
        {
            if (useJsonFormatter)
                builder.AddZLoggerConsole(options =>
                    options.UseJsonFormatter(formatter =>
                        formatter.IncludeProperties = IncludeProperties.ParameterKeyValues));
            else
                builder.AddZLoggerConsole();
        }

        builder.AddZLoggerRollingFile(options =>
        {
            options.RollingInterval = RollingInterval.Day;
            options.RollingSizeKB = rollingSizeKB;
            options.FilePathSelector = (timestamp, sequenceNumber) =>
                $"{rollingFilePath}/{timestamp.ToLocalTime():yyyy-MM-dd}_{sequenceNumber:000}.log";
        });
    }
}
