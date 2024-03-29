using System.Buffers;
using System.Collections.Concurrent;
using System.Text;
using Utf8StringInterpolation;

namespace DropBear.Codex.AppLogger.Factories;

[Obsolete("This class is obsolete and will be removed in a future version. Please use DropBear.Codex.Utilities.MessageTemplateManager instead.")]
public static class MessageTemplateFactory
{
    private static readonly ConcurrentDictionary<string, string> Templates = new(StringComparer.OrdinalIgnoreCase);

    public static void RegisterTemplate(string templateId, string template)
    {
        if (!Templates.TryAdd(templateId, template))
            throw new InvalidOperationException($"A template with the ID '{templateId}' has already been registered.");
    }

    public static byte[] FormatUtf8(string templateId, params object[] args)
    {
        if (!Templates.TryGetValue(templateId, out var template))
            throw new KeyNotFoundException($"No template found with the ID '{templateId}'.");

        var bufferWriter = new ArrayBufferWriter<byte>();
        // Ensuring Utf8StringWriter is disposed correctly using 'using' statement
        using (var writer = Utf8String.CreateWriter(bufferWriter))
        {
            writer.AppendLiteral(template); // Assuming template is a literal part of the formatted message
            foreach (var arg in args) writer.AppendFormatted(arg);
        }

        // No need to manually call Flush() since it's implicitly called by Dispose()
        return bufferWriter.WrittenSpan.ToArray();
    }

    public static string FormatString(string templateId, params object[] args)
    {
        var utf8Bytes = FormatUtf8(templateId, args);
        return Encoding.UTF8.GetString(utf8Bytes);
    }

    public static void RegisterTemplates(Dictionary<string, string> templatesToRegister)
    {
        foreach (var (key, value) in templatesToRegister) RegisterTemplate(key, value);
    }
}
