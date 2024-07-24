#region

using Microsoft.Extensions.Logging;

#endregion

namespace DropBear.Codex.AppLogger.Utils;

/// <summary>
///     Utility class for validating and preparing file paths.
/// </summary>
internal static class FilePathValidator
{
    private static readonly ILogger Logger = LoggerFactory.Create(static builder => builder.AddConsole())
        .CreateLogger("FilePathValidator");

    private static readonly Action<ILogger, Exception> LogInvalidDirectoryPath =
        LoggerMessage.Define(LogLevel.Error, new EventId(1, nameof(LogInvalidDirectoryPath)),
            "Invalid directory path derived from file path.");

    private static readonly Action<ILogger, Exception?> LogDirectoryDoesNotExist =
        LoggerMessage.Define(LogLevel.Information, new EventId(2, nameof(LogDirectoryDoesNotExist)),
            "Directory does not exist, creating directory...");

    private static readonly Action<ILogger, Exception?> LogReadWriteTestPassed =
        LoggerMessage.Define(LogLevel.Information, new EventId(3, nameof(LogReadWriteTestPassed)),
            "Directory and file read/write test passed.");

    private static readonly Action<ILogger, Exception?> LogContentMismatch =
        LoggerMessage.Define(LogLevel.Error, new EventId(4, nameof(LogContentMismatch)),
            "Mismatch in content of written and read files.");

    private static readonly Action<ILogger, Exception> LogValidationError =
        LoggerMessage.Define(LogLevel.Error, new EventId(5, nameof(LogValidationError)),
            "An error occurred during directory validation.");

    /// <summary>
    ///     Validates the file path and ensures the directory exists. Tests read/write access.
    /// </summary>
    /// <param name="filePath">The file path to validate and prepare.</param>
    /// <returns>True if the directory is valid and accessible, otherwise false.</returns>
    public static async Task<bool> ValidateAndPrepareDirectoryAsync(string filePath)
    {
        try
        {
            // Sanitize the file path
            var sanitizedFilePath = Path.GetFullPath(filePath);

            // Extract directory path from the full file path
            var directoryPath = Path.GetDirectoryName(sanitizedFilePath);
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                LogInvalidDirectoryPath(Logger, new ArgumentException("Invalid directory path.", nameof(filePath)));
                return false;
            }

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                LogDirectoryDoesNotExist(Logger, null);
                Directory.CreateDirectory(directoryPath);
            }

            // Test writing and reading a dummy file
            var dummyFilePath = Path.Combine(directoryPath, "dummyFile.txt");
            const string TestContent = "Hello, world!";
            await File.WriteAllTextAsync(dummyFilePath, TestContent).ConfigureAwait(false);
            var readContent = await File.ReadAllTextAsync(dummyFilePath).ConfigureAwait(false);

            // Verify read content matches written content
            if (readContent == TestContent)
            {
                File.Delete(dummyFilePath);
                LogReadWriteTestPassed(Logger, null);
                return true;
            }

            LogContentMismatch(Logger, null);
            return false;
        }
        catch (Exception ex)
        {
            LogValidationError(Logger, ex);
            return false;
        }
    }
}
