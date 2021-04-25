namespace EventHorizon.Zone.System.Wizard.Json.Model
{
    using global::System.Collections.Generic;
    using global::System.Text.Json;

    public struct JsonDataProperty
    {
        public string Name { get; }
        public List<JsonDataProperty> Data { get; set; }
        public string Type { get; }
        public string Value { get; }

        public JsonDataProperty(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
            Data = new List<JsonDataProperty>();
        }

        public void WriteTo(
            Utf8JsonWriter writer
        )
        {
            writer.WritePropertyName(Name);
            if (Type == "String")
            {
                writer.WriteStringValue(Value);
            }
            else if (Type == "Long")
            {
                writer.WriteNumberValue(long.Parse(Value));
            }
            else if (Type == "Boolean")
            {
                writer.WriteBooleanValue(bool.Parse(Value));
            }
            else if (Type == "Object")
            {
                writer.WriteStartObject();
                foreach (var childProperty in Data)
                {
                    childProperty.WriteTo(writer);
                }
                writer.WriteEndObject();
            }
        }
    }
}
