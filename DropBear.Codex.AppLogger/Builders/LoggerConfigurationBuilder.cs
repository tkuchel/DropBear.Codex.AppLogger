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
    private string _filePath; // For MicrosoftLoggerFactory file logging
    private string _logFormat = "{Timestamp:o} [{Level}] {Message}{NewLine}{Exception}"; // For custom log formats
    private LogLevel _logLevel = LogLevel.Information;
    private string _rollingFilePath = "logs/";
    private int _rollingSizeKB = 1024; // Default to 1 MB
    private bool _useJsonFormatter;


    public LoggerConfigurationBuilder UseJsonFormatter(bool useJson = true)
    {
        _useJsonFormatter = useJson;
        return this;
    }

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

    public LoggerConfigurationBuilder SetOutputFilePath(string filePath)
    {
        _filePath = filePath;
        return this;
    }

    public LoggerConfigurationBuilder SetLogFormat(string logFormat)
    {
        _logFormat = logFormat;
        return this;
    }

    public ILoggingFactory Build()
    {
        // Choose the factory based on specific conditions or configurations
        // Example logic can be included here to decide between MicrosoftLoggerFactory and ZLoggerFactory
        if (_useJsonFormatter || !string.IsNullOrEmpty(_rollingFilePath))
            // Use ZLoggerFactory for JSON formatting and rolling file support
            return new ZLoggerFactory(_logLevel, _consoleOutput, _rollingFilePath, _rollingSizeKB, _useJsonFormatter);
        // Default to MicrosoftLoggerFactory, now supporting file path and custom log formats
        return new MicrosoftLoggerFactory(_logLevel, _consoleOutput, _filePath, _logFormat);
    }
}
