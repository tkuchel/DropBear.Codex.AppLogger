namespace DropBear.Codex.AppLogger.Utils;

public static class FilePathValidator
{
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
                Console.WriteLine("Invalid directory path derived from file path.");
                return false;
            }

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist, creating directory...");
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
                Console.WriteLine("Directory and file read/write test passed.");
                return true;
            }

            Console.WriteLine("Mismatch in content of written and read files.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }
}
