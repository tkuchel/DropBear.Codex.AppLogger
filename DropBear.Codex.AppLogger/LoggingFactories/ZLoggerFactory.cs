using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Providers;

namespace DropBear.Codex.AppLogger.LoggingFactories;

/// <summary>
///     Concrete logging factory for ZLogger.
/// </summary>
public class ZLoggerFactory : ILoggingFactory
{
    private readonly ILoggerFactory _loggerFactory;

    // ReSharper disable once InconsistentNaming
    public ZLoggerFactory(LogLevel logLevel, bool consoleOutput, int rollingSizeKB, bool useJsonFormatter) =>
        // Configure ILoggerFactory here based on the provided settings
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            ConfigureZLogger(builder, logLevel, consoleOutput, rollingSizeKB,
                useJsonFormatter);
        });

    public ILogger CreateLogger<T>() =>
        // Use the pre-configured ILoggerFactory to create logger instances
        _loggerFactory.CreateLogger<T>();

    private static void ConfigureZLogger(ILoggingBuilder builder, LogLevel logLevel,
        bool consoleOutput,
        // ReSharper disable once InconsistentNaming
        int rollingSizeKB, bool useJsonFormatter)
    {
        builder.ClearProviders()
            .SetMinimumLevel(logLevel);

        if (consoleOutput && useJsonFormatter)
            builder.AddZLoggerConsole(options =>
            {
                options.UseJsonFormatter(formatter =>
                {
                    formatter.IncludeProperties = IncludeProperties.ParameterKeyValues;
                });
            });

        builder.AddZLoggerRollingFile(x =>
        {
            x.RollingInterval = RollingInterval.Day;
            x.RollingSizeKB = rollingSizeKB;
            x.FilePathSelector = (timestamp, sequenceNumber) =>
                $"logs/{timestamp.ToLocalTime():yyyy-MM-dd}_{sequenceNumber:000}.log";
        });
    }
}
