using Microsoft.Extensions.Logging;

namespace DropBear.Codex.AppLogger.Utils;

/// <summary>
///     Utility class for validating and preparing file paths.
/// </summary>
public static class FilePathValidator
{
    private static readonly ILogger Logger = LoggerFactory.Create(builder => builder.AddConsole())
        .CreateLogger("FilePathValidator");

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
                Logger.LogError("Invalid directory path derived from file path.");
                return false;
            }

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Logger.LogInformation("Directory does not exist, creating directory...");
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
                Logger.LogInformation("Directory and file read/write test passed.");
                return true;
            }

            Logger.LogError("Mismatch in content of written and read files.");
            return false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred during directory validation.");
            return false;
        }
    }
}
