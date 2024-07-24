#region

using System.Text;
using DropBear.Codex.AppLogger.LoggingFactories;
using DropBear.Codex.AppLogger.Utils;
using DropBear.Codex.AppLogger.Wrappers;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

#endregion

namespace DropBear.Codex.AppLogger.Builders;

/// <summary>
///     Builder for fluent logger configuration.
/// </summary>
public sealed class LoggerConfigurationBuilder
{
    private bool _consoleOutput = true;
    private LogLevel _logLevel = LogLevel.Information;
    private string _rollingFilePath = "logs/";
    private int _rollingSizeKb = 1024; // Default to 1 MB
    private bool _useJsonFormatter;
    private bool _useSerilog;

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
        {
            throw new ArgumentException("Rolling file path cannot be null or empty", nameof(_rollingFilePath));
        }
#pragma warning restore MA0015
#pragma warning restore CA2208

        var directoryExists =
            await FilePathValidator.ValidateAndPrepareDirectoryAsync(_rollingFilePath).ConfigureAwait(false);
        if (!directoryExists)
        {
            throw new DirectoryNotFoundException($"Directory {_rollingFilePath} does not exist");
        }

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

    public LoggerConfigurationBuilder UseSerilog(bool useSerilog = true)
    {
        _useSerilog = useSerilog;
        return this;
    }

    public ILoggerFactory Build()
    {
        if (_useSerilog)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(ConvertLogLevel(_logLevel));

            if (_consoleOutput)
            {
                if (_useJsonFormatter)
                {
                    loggerConfiguration.WriteTo.Console(new JsonFormatter());
                }
                else
                {
                    loggerConfiguration.WriteTo.Console();
                }
            }

            if (!string.IsNullOrEmpty(_rollingFilePath))
            {
                if (_useJsonFormatter)
                {
                    loggerConfiguration.WriteTo.File(
                        new JsonFormatter(),
                        _rollingFilePath,
                        ConvertLogLevel(_logLevel),
                        _rollingSizeKb * 1024,
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: 7,
                        encoding: Encoding.UTF8);
                }
                else
                {
                    loggerConfiguration.WriteTo.File(
                        _rollingFilePath,
                        ConvertLogLevel(_logLevel),
                        fileSizeLimitBytes: _rollingSizeKb * 1024,
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: 7,
                        encoding: Encoding.UTF8);
                }
            }

            var serilogLogger = loggerConfiguration.CreateLogger();
            return new SerilogLoggerFactoryWrapper(new SerilogLoggerFactory(serilogLogger));
        }

        return new ZLoggerFactory(_logLevel, _consoleOutput, _rollingFilePath, _rollingSizeKb, _useJsonFormatter);
    }

    private static LogEventLevel ConvertLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.None => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }
}
