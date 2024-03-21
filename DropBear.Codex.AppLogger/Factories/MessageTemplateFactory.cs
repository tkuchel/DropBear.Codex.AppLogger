using System.Collections.Concurrent;
using Cysharp.Text;

namespace DropBear.Codex.AppLogger.Factories;

public static class MessageTemplateFactory
{
    private static readonly ConcurrentDictionary<string, string> Templates = new(StringComparer.OrdinalIgnoreCase);

    // ReSharper disable once MemberCanBePrivate.Global
    public static void RegisterTemplate(string templateId, string template)
    {
        if (!Templates.TryAdd(templateId, template))
            throw new InvalidOperationException($"A template with the ID '{templateId}' has already been registered.");
    }

    public static string Format(string templateId, params object[] args)
    {
        if (!Templates.TryGetValue(templateId, out var template))
            throw new KeyNotFoundException($"No template found with the ID '{templateId}'.");

        try
        {
            return ZString.Format(template, args);
        }
        catch (Exception ex)
        {
            throw new FormatException($"Error formatting the template '{templateId}'.", ex);
        }
    }

    // Optional: Method to bulk register templates, e.g., from configuration files or startup routines.
    public static void RegisterTemplates(Dictionary<string, string> templatesToRegister)
    {
        foreach (var pair in templatesToRegister) RegisterTemplate(pair.Key, pair.Value);
    }
}
