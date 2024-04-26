using DropBear.Codex.AppLogger.LoggingFactories;
using DropBear.Codex.AppLogger.Utils;
using Microsoft.Extensions.Logging;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

namespace DropBear.Codex.AppLogger.Builders;

/// <summary>
///     Builder for fluent logger configuration.
/// </summary>
public class LoggerConfigurationBuilder
{
    private bool _consoleOutput = true;
    private LogLevel _logLevel = LogLevel.Information;
    private string _rollingFilePath = "logs/";
    private int _rollingSizeKb = 1024; // Default to 1 MB
    private bool _useJsonFormatter;

    public LoggerConfigurationBuilder UseJsonFormatter(bool useJson = true)
    {
        _useJsonFormatter = useJson;
        return this;
    }

    public async Task<LoggerConfigurationBuilder> ConfigureRollingFileAsync(string path, int sizeKB)
    {
        _rollingFilePath = path;
        _rollingSizeKb = sizeKB;

        if (string.IsNullOrEmpty(_rollingFilePath))
#pragma warning disable CA2208
#pragma warning disable MA0015
            throw new ArgumentException("Rolling file path cannot be null or empty", nameof(_rollingFilePath));
#pragma warning restore MA0015
#pragma warning restore CA2208

        var directoryExists = await FilePathValidator.ValidateAndPrepareDirectoryAsync(_rollingFilePath).ConfigureAwait(false);
        if (!directoryExists)
            throw new DirectoryNotFoundException($"Directory {_rollingFilePath} does not exist");

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

    public ILoggerFactory Build() =>
        new ZLoggerFactory(_logLevel, _consoleOutput, _rollingFilePath, _rollingSizeKb, _useJsonFormatter);
}
