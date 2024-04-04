namespace EventHorizon.Zone.System.Wizard.Json.Model;

using global::System;
using global::System.Buffers;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Text;
using global::System.Text.Json;

public struct JsonDataDocument(IList<JsonDataProperty>? properties)
{
    public IEnumerable<JsonDataProperty>? Properties { get; } = properties ??= [];

    public readonly string ToJsonString()
    {
        var outputBuffer = new ArrayBufferWriter<byte>();
        using (var jsonWriter = new Utf8JsonWriter(outputBuffer))
        {
            jsonWriter.WriteStartObject();

            foreach (var jsonProperty in Properties ?? [])
            {
                jsonProperty.WriteTo(jsonWriter);
            }

            jsonWriter.WriteEndObject();
        }

        return Encoding.UTF8.GetString(outputBuffer.WrittenSpan);
    }
}
