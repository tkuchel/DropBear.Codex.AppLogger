#region

using System.Collections.Concurrent;
using DropBear.Codex.AppLogger.Builders;
using Microsoft.Extensions.Logging;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

#endregion

namespace DropBear.Codex.AppLogger;

public sealed class LoggerManager
{
#pragma warning disable IDE1006
    // ReSharper disable once InconsistentNaming
    private static readonly Lazy<LoggerManager> _instance = new(static () => new LoggerManager());
#pragma warning restore IDE1006
    private readonly ConcurrentDictionary<string, ILogger?> _loggerCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<Type, ILogger> _loggerTypeCache = new();
    private ILoggerFactory _loggerFactory;

    private LoggerManager()
    {
        var builder = new LoggerConfigurationBuilder()
            .SetLogLevel(LogLevel.Information)
            .EnableConsoleOutput();
        _loggerFactory = builder.Build();
    }

    public static LoggerManager Instance => _instance.Value;

    public ILogger? GetLogger(string category)
    {
        return _loggerCache.GetOrAdd(category, _loggerFactory.CreateLogger);
    }

    public ILogger<T> GetLogger<T>()
    {
        var type = typeof(T);
        return (ILogger<T>)_loggerTypeCache.GetOrAdd(type, CreateTypedLogger<T>);
    }

    private ILogger CreateTypedLogger<T>(Type type)
    {
        return _loggerFactory.CreateLogger<T>();
    }

    public void ConfigureLogger(Action<LoggerConfigurationBuilder> configure)
    {
        var builder = new LoggerConfigurationBuilder();
        configure(builder);
        var newFactory = builder.Build();
        UpdateLoggerFactory(newFactory);
    }

    private void UpdateLoggerFactory(ILoggerFactory newFactory)
    {
        if (_loggerFactory is IDisposable disposable)
        {
            disposable.Dispose();
        }

        _loggerFactory = newFactory;
        _loggerCache.Clear();
        _loggerTypeCache.Clear();
    }
}
