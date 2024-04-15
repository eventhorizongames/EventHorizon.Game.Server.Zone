namespace EventHorizon.Zone.System.Server.Scripts.Parsers;

using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Server.Scripts.Model;
using global::System.Collections.Generic;
using global::System.Text.Json;

public class StandardDataParsers : DataParsers
{
    public T DeserializeFromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, IJsonFileSaver.DEFAULT_JSON_OPTIONS)!;
    }

    public IDictionary<string, string> FlattenJsonDocumentParser(JsonDocument jsonDocument)
    {
        return RawJsonDataParser.Parse(jsonDocument);
    }

    public string SerializeToJson<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, IJsonFileSaver.DEFAULT_JSON_OPTIONS);
    }
}
