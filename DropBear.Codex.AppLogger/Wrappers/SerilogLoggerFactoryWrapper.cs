#region

using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using ILoggerFactory = DropBear.Codex.AppLogger.Interfaces.ILoggerFactory;

#endregion

namespace DropBear.Codex.AppLogger.Wrappers;

internal sealed class SerilogLoggerFactoryWrapper : ILoggerFactory
{
    private readonly SerilogLoggerFactory _serilogFactory;

    public SerilogLoggerFactoryWrapper(SerilogLoggerFactory serilogFactory)
    {
        _serilogFactory = serilogFactory;
    }

    public ILogger<T> CreateLogger<T>()
    {
        return _serilogFactory.CreateLogger<T>();
    }

    public ILogger? CreateLogger(string categoryName)
    {
        return _serilogFactory.CreateLogger(categoryName);
    }

    public void Dispose()
    {
        _serilogFactory.Dispose();
    }
}
