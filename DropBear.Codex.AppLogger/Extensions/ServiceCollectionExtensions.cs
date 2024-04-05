﻿using DropBear.Codex.AppLogger.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Extensions;

/// <summary>
///     Extensions for configuring application logging services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds application logger services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add logging services to.</param>
    /// <param name="configure">The logger configuration builder configuration action.</param>
    /// <returns>The modified <see cref="IServiceCollection" /> with added logging services.</returns>
    public static IServiceCollection AddAppLogger(this IServiceCollection services,
        Action<LoggerConfigurationBuilder> configure)
    {
        // Create a new instance of LoggerConfigurationBuilder
        var builder = new LoggerConfigurationBuilder();
        // Apply the user-provided configurations
        configure(builder);

        // Build the appropriate ILoggerFactory based on the configuration
        var loggerFactory = builder.Build();

        // Register the factory with the DI container.
        // Since loggerFactory is an ILoggingFactory, but services expect an ILoggerFactory,
        // ensure your ILoggingFactory implementation is compatible or adapt as necessary.
        services.AddSingleton<ILoggerFactory>(serviceProvider =>
        {
            // This assumes your ILoggingFactory implementation somehow provides an ILoggerFactory.
            // Adjust this line if your implementation differs.
            return loggerFactory as ILoggerFactory;
        });

        // Optional: register ILogger<T> to be resolved via the factory
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        return services;
    }
}
