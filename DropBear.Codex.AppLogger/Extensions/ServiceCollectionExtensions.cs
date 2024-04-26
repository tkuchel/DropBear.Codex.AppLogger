using DropBear.Codex.AppLogger.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

namespace DropBear.Codex.AppLogger.Extensions;

/// <summary>
///     Extensions for configuring application logging services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds application logger services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add logging services to.</param>
    /// <param name="configure">The logger configuration builder action.</param>
    /// <returns>The modified IServiceCollection with added logging services.</returns>
    public static IServiceCollection AddAppLogger(this IServiceCollection services,
        Action<LoggerConfigurationBuilder> configure)
    {
        var builder = new LoggerConfigurationBuilder();
        configure(builder);
        var loggerFactory = builder.Build();

        services.AddSingleton(loggerFactory);
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        return services;
    }
    
    // Making Logger<T> static if it's only used for extension purposes
    private static class Logger<T> where T : class
    {
        // Static members to facilitate logging without direct instantiation
    }

    // private sealed class Logger<T> : ILogger<T>, IDisposable
    // {
    //     private readonly ILogger? _logger;
    //     private bool _disposed;
    //
    //     public Logger(ILoggerFactory factory)
    //     {
    //         var categoryName = typeof(T).FullName;
    //         if (categoryName is not null) _logger = factory.CreateLogger(categoryName);
    //     }
    //
    //     public void Dispose()
    //     {
    //         Dispose(disposing: true);
    //         GC.SuppressFinalize(this);
    //     }
    //
    //     public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
    //         Func<TState, Exception?, string> formatter) => _logger?.Log(logLevel, eventId, state, exception, formatter);
    //
    //     public bool IsEnabled(LogLevel logLevel) => _logger is not null && _logger.IsEnabled(logLevel);
    //
    //     public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _logger?.BeginScope(state);
    //
    //     private void Dispose(bool disposing)
    //     {
    //         if (_disposed) return;
    //         if (disposing) (_logger as IDisposable)?.Dispose();
    //         _disposed = true;
    //     }
    //
    //     ~Logger() => Dispose(disposing: false);
    // }
}
