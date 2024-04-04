namespace EventHorizon.Zone.System.Server.Scripts.Parsers;

using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Text.Json;

public static class RawJsonDataParser
{
    public static Dictionary<string, string> Parse(JsonDocument jsonDocument)
    {
        var data = new Dictionary<string, string>();

        foreach (var item in jsonDocument.RootElement.EnumerateObject())
        {
            data = GetDataProperties(data, item.Name, item.Value);
        }

        return data;
    }

    private static readonly JsonValueKind[] PrimitiveTypes =
    [
        JsonValueKind.Number,
        JsonValueKind.True,
        JsonValueKind.False
    ];

    public static Dictionary<string, string> GetDataProperties(
        Dictionary<string, string> data,
        string jsonPropertyName,
        JsonElement jsonElement,
        string? parentPropertyName = null
    )
    {
        var key = NormalizeForEditor(jsonPropertyName);
        if (parentPropertyName is not null)
        {
            key = $"{parentPropertyName}:{key}";
        }

        if (jsonElement.ValueKind == JsonValueKind.String)
        {
            data[key] = jsonElement.GetString() ?? string.Empty;
        }
        else if (PrimitiveTypes.Any(a => a == jsonElement.ValueKind))
        {
            data[key] = jsonElement.GetRawText();
        }
        else if (jsonElement.ValueKind == JsonValueKind.Array)
        {
            var index = 0;
            foreach (var arrayItem in jsonElement.EnumerateArray())
            {
                GetDataProperties(data, $"[{index}]", arrayItem, key);
                index++;
            }
        }
        else if (jsonElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var childProperty in jsonElement.EnumerateObject())
            {
                data = GetDataProperties(data, childProperty.Name, childProperty.Value, key);
            }
        }
        return data;
    }

    public static string NormalizeForEditor(string key)
    {
        return char.ToLower(key.First()) + key[1..];
    }
}
