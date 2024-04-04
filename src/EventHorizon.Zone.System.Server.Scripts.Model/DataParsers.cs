namespace EventHorizon.Zone.System.Server.Scripts.Model;

using global::System.Collections.Generic;
using global::System.Text.Json;

public interface DataParsers
{
    public IDictionary<string, string> FlattenJsonDocumentParser(JsonDocument jsonDocument);

    public string SerializeToJson<T>(T obj);
    public T DeserializeFromJson<T>(string json);
}
