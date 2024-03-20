using System.Globalization;
using System.Text;
using DropBear.Codex.AppLogger.Interfaces;
using DropBear.Codex.AppLogger.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Providers;

namespace DropBear.Codex.AppLogger.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppLogger(this IServiceCollection services)
    {
        // Check if an ILogger<T> is already configured, considering any T
        var loggerExists = services.Any(service =>
            service.ServiceType.IsGenericType &&
            service.ServiceType.GetGenericTypeDefinition() == typeof(ILogger<>));

        if (!loggerExists)
            ConfigureZLogger(services);

        return AddLoggingAdapter(services);
    }


    private static IServiceCollection AddLoggingAdapter(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
        return services;
    }

    /// <summary>
    ///     Configures ZLogger logging services.
    /// </summary>
    /// <param name="services">The IServiceCollection to add logging services to.</param>
    private static void ConfigureZLogger(IServiceCollection services) =>
        services.AddLogging(builder =>
        {
            builder.ClearProviders()
                .SetMinimumLevel(LogLevel.Debug)
                .AddZLoggerConsole(options =>
                {
                    options.UseJsonFormatter(formatter =>
                    {
                        formatter.IncludeProperties = IncludeProperties.ParameterKeyValues;
                    });
                })
                .AddZLoggerRollingFile(options =>
                {
                    options.FilePathSelector = (timestamp, sequenceNumber) => new StringBuilder()
                        .Append("logs/")
                        .Append(timestamp.ToLocalTime().ToString("yyyy-MM-dd", CultureInfo.CurrentCulture))
                        .Append('_')
                        .Append(sequenceNumber.ToString("000", CultureInfo.CurrentCulture))
                        .Append(".log")
                        .ToString();

                    options.RollingInterval = RollingInterval.Day;
                    options.RollingSizeKB = 1024; // 1MB
                });
        });
}
