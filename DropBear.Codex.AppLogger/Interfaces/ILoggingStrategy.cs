namespace DropBear.Codex.AppLogger.Interfaces;

public interface ILoggingStrategy
{
    void LogDebug(string message);
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? exception = null);
    void LogCritical(string message, Exception? exception = null);
}

