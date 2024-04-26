using System.Collections.Concurrent;
using DropBear.Codex.AppLogger.Builders;
using Microsoft.Extensions.Logging;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;


namespace DropBear.Codex.AppLogger;

/// <summary>
///     Manages and caches logger instances as a singleton.
/// </summary>
public sealed class LoggerManager
{
#pragma warning disable IDE1006 // Naming Styles
    // ReSharper disable once InconsistentNaming
    private static readonly Lazy<LoggerManager> _instance = new(() => new LoggerManager());
#pragma warning restore IDE1006 // Naming Styles
    private readonly ConcurrentDictionary<string, ILogger?> _loggerCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<Type, ILogger> _loggerTypeCache = new();
    internal ILoggerFactory _loggerFactory;

    private LoggerManager()
    {
        var builder = new LoggerConfigurationBuilder()
            .SetLogLevel(LogLevel.Information)
            .EnableConsoleOutput();
        _loggerFactory = builder.Build();
    }

    public static LoggerManager Instance => _instance.Value;

    // Avoiding closure creation by using a method group
    public ILogger? GetLogger(string category) =>
        _loggerCache.GetOrAdd(category, _loggerFactory.CreateLogger);

    public ILogger<T> GetLogger<T>()
    {
        var type = typeof(T);
        // Avoiding closure by passing a factory method that handles the creation
        return (ILogger<T>)_loggerTypeCache.GetOrAdd(type, CreateTypedLogger<T>);
    }

    // Helper method to create logger to avoid closures in GetLogger<T>
    private ILogger CreateTypedLogger<T>(Type type) => _loggerFactory.CreateLogger<T>();

    /// <summary>
    ///     Allows runtime changes in the logger configuration.
    /// </summary>
    /// <param name="configure">A configuration action to apply to the logger configuration builder.</param>
    public void ConfigureLogger(Action<LoggerConfigurationBuilder> configure)
    {
        var builder = new LoggerConfigurationBuilder();
        configure(builder);
        var newFactory = builder.Build();
        UpdateLoggerFactory(newFactory);
    }

    private void UpdateLoggerFactory(ILoggerFactory newFactory)
    {
        // Dispose of the old factory and clear caches
        if (_loggerFactory is IDisposable disposable) disposable.Dispose();

        _loggerFactory = newFactory;
        _loggerCache.Clear();
        _loggerTypeCache.Clear();
    }
}
