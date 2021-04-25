namespace EventHorizon.Zone.System.Wizard.Json.Model
{
    using global::System;
    using global::System.Buffers;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Text;
    using global::System.Text.Json;

    public struct JsonDataDocument
    {
        private static readonly IEnumerable<JsonDataProperty> EMPTY_PROPERTY_LIST = Array.Empty<JsonDataProperty>().ToList();

        public IList<JsonDataProperty> Properties { get; }

        public JsonDataDocument(
            IList<JsonDataProperty> properties
        )
        {
            Properties = properties;
        }

        public string ToJsonString()
        {
            var outputBuffer = new ArrayBufferWriter<byte>();
            using (var jsonWriter = new Utf8JsonWriter(
                outputBuffer
            ))
            {

                jsonWriter.WriteStartObject();

                foreach (var jsonProperty in Properties ?? EMPTY_PROPERTY_LIST)
                {
                    jsonProperty.WriteTo(jsonWriter);
                }

                jsonWriter.WriteEndObject();
            }

            return Encoding.UTF8.GetString(
                outputBuffer.WrittenSpan
            );
        }

    }
}

