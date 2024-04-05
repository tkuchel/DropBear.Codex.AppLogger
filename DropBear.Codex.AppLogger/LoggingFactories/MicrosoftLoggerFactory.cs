using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.LoggingFactories;

public class MicrosoftLoggerFactory : ILoggingFactory, IDisposable
{
    private readonly ILoggerFactory _loggerFactory;
    private bool _disposed;

    public MicrosoftLoggerFactory(LogLevel logLevel, bool consoleOutput) =>
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders()
                .SetMinimumLevel(logLevel);

            if (consoleOutput) builder.AddConsole();
            // File logging and custom format logic here
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

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);
}
