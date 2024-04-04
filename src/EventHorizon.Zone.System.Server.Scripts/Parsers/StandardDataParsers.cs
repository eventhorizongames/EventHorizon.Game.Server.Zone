namespace EventHorizon.Zone.System.Server.Scripts.Parsers;

using EventHorizon.Zone.System.Server.Scripts.Model;
using global::System.Collections.Generic;
using global::System.Text.Json;

public class StandardDataParsers : DataParsers
{
    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true, WriteIndented = true, };

    public T DeserializeFromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonOptions)!;
    }

    public IDictionary<string, string> FlattenJsonDocumentParser(JsonDocument jsonDocument)
    {
        return RawJsonDataParser.Parse(jsonDocument);
    }

    public string SerializeToJson<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, JsonOptions);
    }
}
