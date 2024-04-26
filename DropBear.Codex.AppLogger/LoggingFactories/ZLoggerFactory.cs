
using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Providers;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

namespace DropBear.Codex.AppLogger.LoggingFactories;

public class ZLoggerFactory : ILoggerFactory, IDisposable
{
    private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;
    private bool _disposed;

    public ZLoggerFactory(LogLevel logLevel, bool consoleOutput, string rollingFilePath, int rollingSizeKB,
        bool useJsonFormatter) =>
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            if (string.IsNullOrEmpty(rollingFilePath))
                throw new ArgumentNullException(nameof(rollingFilePath), "Rolling file path cannot be null or empty");

            if (!Directory.Exists(rollingFilePath))
                throw new DirectoryNotFoundException($"Directory {rollingFilePath} does not exist");

            ConfigureZLogger(builder, logLevel, consoleOutput, rollingFilePath, rollingSizeKB, useJsonFormatter);
        });

    public void Dispose()
    {
        if (_disposed) return;
        _loggerFactory.Dispose();
        _disposed = true;
    }

    public ILogger<T> CreateLogger<T>()
    {
        ThrowIfDisposed();
        return _loggerFactory.CreateLogger<T>();
    }

    public ILogger CreateLogger(string categoryName)
    {
        ThrowIfDisposed();
        return _loggerFactory.CreateLogger(categoryName);
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    private static void ConfigureZLogger(ILoggingBuilder builder, LogLevel logLevel, bool consoleOutput,
        string rollingFilePath, int rollingSizeKB, bool useJsonFormatter)
    {
        builder.ClearProviders()
            .SetMinimumLevel(logLevel);

        switch (consoleOutput)
        {
            case true when useJsonFormatter:
                builder.AddZLoggerConsole(options =>
                {
                    options.UseJsonFormatter(formatter =>
                    {
                        formatter.IncludeProperties = IncludeProperties.ParameterKeyValues;
                    });
                });
                break;
            case true:
                builder.AddZLoggerConsole();
                break;
        }

        builder.AddZLoggerRollingFile(x =>
        {
            x.RollingInterval = RollingInterval.Day;
            x.RollingSizeKB = rollingSizeKB;
            x.FilePathSelector = (timestamp, sequenceNumber) =>
                $"{rollingFilePath}/{timestamp.ToLocalTime():yyyy-MM-dd}_{sequenceNumber:000}.log";
        });
    }
}
