namespace EventHorizon.Zone.System.Wizard.Json.Model;

using global::System.Collections.Generic;
using global::System.Text.Json;

public struct JsonDataProperty
{
    public string Name { get; }
    public List<JsonDataProperty> Data { get; set; }
    public string Type { get; }
    public string Value { get; }
    public bool IncludeName { get; }

    public JsonDataProperty(string name, string type, string value, bool includeName = true)
    {
        Name = name;
        Type = type;
        Value = value;
        Data = [];
        IncludeName = includeName;
    }

    public void WriteTo(Utf8JsonWriter writer)
    {
        if (IncludeName)
        {
            writer.WritePropertyName(Name);
        }

        switch (Type)
        {
            case "String":
                writer.WriteStringValue(Value);
                break;
            case "Long":
                writer.WriteNumberValue(long.Parse(Value));
                break;
            case "Boolean":
                writer.WriteBooleanValue(bool.Parse(Value));
                break;
            case "Object":
            {
                writer.WriteStartObject();
                foreach (var childProperty in Data)
                {
                    childProperty.WriteTo(writer);
                }
                writer.WriteEndObject();
                break;
            }
            case "InputKeyMap":
            {
                writer.WriteStartObject();
                foreach (var childProperty in Data)
                {
                    childProperty.WriteTo(writer);
                }
                writer.WriteEndObject();
                break;
            }
            default:
                writer.WriteStringValue(Value);
                break;
        }
    }
}
