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

        // Register the custom logger factory as a singleton
        services.AddSingleton(loggerFactory);

        // Register ILogger<> implementation using a custom Logger<> class
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        return services;
    }

    /// <summary>
    ///     Logger implementation for ILogger<T>.
    /// </summary>
    /// <typeparam name="T">The type for which the logger is being created.</typeparam>
    private sealed class Logger<T> : ILogger<T>
    {
        private readonly ILogger? _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Logger{T}" /> class.
        /// </summary>
        /// <param name="factory">The custom logger factory.</param>
        public Logger(ILoggerFactory factory)
        {
            var categoryName = typeof(T).FullName;
            _logger = factory.CreateLogger(categoryName ?? typeof(T).Name);
        }

        /// <inheritdoc />
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            _logger?.BeginScope(state);

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => _logger is not null && _logger.IsEnabled(logLevel);

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            ArgumentNullException.ThrowIfNull(formatter);
            _logger?.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
