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


    // ReSharper disable once UnusedMember.Global
    public LoggerConfigurationBuilder UseJsonFormatter(bool useJson = true)
    {
        _useJsonFormatter = useJson;
        return this;
    }

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Global
    public LoggerConfigurationBuilder ConfigureRollingFile(string path, int sizeKB)
    {
        _rollingFilePath = path;
        _rollingSizeKB = sizeKB;
        return this;
    }

    // ReSharper disable once UnusedMember.Global
    public LoggerConfigurationBuilder SetLogLevel(LogLevel logLevel)
    {
        _logLevel = logLevel;
        return this;
    }

    // ReSharper disable once UnusedMember.Global
    public LoggerConfigurationBuilder EnableConsoleOutput(bool enable = true)
    {
        _consoleOutput = enable;
        return this;
    }

    public ILoggingFactory Build()
    {
            return new ZLoggerFactory(_logLevel, _consoleOutput,_rollingFilePath, _rollingSizeKB, _useJsonFormatter);
    }
}
