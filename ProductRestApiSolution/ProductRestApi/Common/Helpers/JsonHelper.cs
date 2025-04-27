using System.Text.Json;

namespace ProductRestApi.Common.Helpers;

public static class JsonHelper
{
    public static object? TryParseJson(string input)
    {
        try
        {
            return JsonSerializer.Deserialize<object>(input);
        }
        catch
        {
            return null;
        }
    }
    
    public static string? PrettyPrintJson(string input)
    {
        try
        {
            using var jDoc = JsonDocument.Parse(input);
            return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch
        {
            return input;
        }
    }
}