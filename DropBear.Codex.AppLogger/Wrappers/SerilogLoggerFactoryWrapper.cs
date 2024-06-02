using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

namespace DropBear.Codex.AppLogger.Wrappers;

public class SerilogLoggerFactoryWrapper : ILoggerFactory
{
    private readonly SerilogLoggerFactory _serilogFactory;

    public SerilogLoggerFactoryWrapper(SerilogLoggerFactory serilogFactory) => _serilogFactory = serilogFactory;

    public ILogger<T> CreateLogger<T>() => _serilogFactory.CreateLogger<T>();

    public ILogger? CreateLogger(string categoryName) => _serilogFactory.CreateLogger(categoryName);

    public void Dispose() => _serilogFactory.Dispose();
}
