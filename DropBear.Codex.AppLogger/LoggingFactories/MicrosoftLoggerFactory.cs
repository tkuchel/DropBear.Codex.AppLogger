﻿using DropBear.Codex.AppLogger.Interfaces;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.LoggingFactories;

/// <summary>
///     Concrete logging factory for Microsoft.Extensions.Logging.
/// </summary>
public class MicrosoftLoggerFactory : ILoggingFactory
{
    private readonly bool _consoleOutput;
    private readonly string _filePath;
    private readonly string _logFormat;
    private readonly LogLevel _logLevel;

    public MicrosoftLoggerFactory(LogLevel logLevel, bool consoleOutput, string filePath, string logFormat)
    {
        _logLevel = logLevel;
        _consoleOutput = consoleOutput;
        _filePath = filePath;
        _logFormat = logFormat;
    }

    public ILogger CreateLogger<T>()
    {
#pragma warning disable CA2000
        var factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders()
                .SetMinimumLevel(_logLevel);

            if (_consoleOutput) builder.AddConsole();
            // Additional logic for custom log format if applicable
            if (!string.IsNullOrEmpty(_filePath))
            {
                // Integrate file logging based on _filePath
                // This might require a custom extension method or third-party library
                // as Microsoft.Extensions.Logging does not provide built-in file logging
                
            }

            // Apply custom log format if specified
            // Note: Custom formatting application will depend on the capabilities of the logging provider
        });
#pragma warning restore CA2000

        return factory.CreateLogger<T>();
    }
}
