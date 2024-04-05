using DropBear.Codex.AppLogger.Interfaces;
using DropBear.Codex.AppLogger.LoggingFactories;
using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Builders;

/// <summary>
///     Builder for fluent logger configuration.
/// </summary>
public class LoggerConfigurationBuilder
{
    private bool _consoleOutput = true;
    private LogLevel _logLevel = LogLevel.Information;
    private string _rollingFilePath = "logs/";

    // ReSharper disable once InconsistentNaming
    private int _rollingSizeKB = 1024; // Default to 1 MB
    private bool _useJsonFormatter;


    public LoggerConfigurationBuilder UseJsonFormatter(bool useJson = true)
    {
        _useJsonFormatter = useJson;
        return this;
    }

    // ReSharper disable once InconsistentNaming
    public LoggerConfigurationBuilder ConfigureRollingFile(string path, int sizeKB)
    {
        _rollingFilePath = path;
        _rollingSizeKB = sizeKB;
        return this;
    }

    public LoggerConfigurationBuilder SetLogLevel(LogLevel logLevel)
    {
        _logLevel = logLevel;
        return this;
    }

    public LoggerConfigurationBuilder EnableConsoleOutput(bool enable = true)
    {
        _consoleOutput = enable;
        return this;
    }

    public ILoggingFactory Build()
    {
        // Choose the factory based on specific conditions or configurations
        if (_useJsonFormatter || !string.IsNullOrEmpty(_rollingFilePath))
            // Use ZLoggerFactory for JSON formatting and rolling file support
            // Assuming ZLoggerFactory's constructor is adjusted to accept logFormat
            return new ZLoggerFactory(_logLevel, _consoleOutput,_rollingFilePath, _rollingSizeKB, _useJsonFormatter);
        // Default to MicrosoftLoggerFactory, now supporting file path and custom log formats
        // Assuming MicrosoftLoggerFactory's constructor is adjusted to accept logFormat
        return new MicrosoftLoggerFactory(_logLevel, _consoleOutput);
    }
}
