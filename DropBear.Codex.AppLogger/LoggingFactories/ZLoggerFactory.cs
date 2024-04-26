using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Providers;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

namespace DropBear.Codex.AppLogger.LoggingFactories;

public sealed class ZLoggerFactory : ILoggerFactory, IDisposable
{
    private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;
    private bool _disposed;

    public ZLoggerFactory(LogLevel logLevel, bool consoleOutput, string rollingFilePath, int rollingSizeKB,
        bool useJsonFormatter) =>
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            if (string.IsNullOrEmpty(rollingFilePath))
                throw new ArgumentException("Rolling file path cannot be empty", nameof(rollingFilePath));

            if (!Directory.Exists(rollingFilePath))
                throw new DirectoryNotFoundException($"Directory {rollingFilePath} does not exist");

            ConfigureZLogger(builder, logLevel, consoleOutput, rollingFilePath, rollingSizeKB, useJsonFormatter);
        });

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ILogger<T> CreateLogger<T>()
    {
        ThrowIfDisposed();
        return _loggerFactory.CreateLogger<T>();
    }

    public ILogger? CreateLogger(string categoryName)
    {
        ThrowIfDisposed();
        return _loggerFactory.CreateLogger(categoryName);
    }

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
                builder.AddZLoggerConsole(options => options.UseJsonFormatter(formatter =>
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
