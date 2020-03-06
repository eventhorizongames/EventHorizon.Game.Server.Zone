namespace EventHorizon.Zone.Core.Model.Entity
{
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using System.Text.Json;
    using System.Buffers;
    using System;

    public static class DataPropertyExtensions
    {
        private static JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static T GetProperty<T>(
            this IObjectEntity entity,
            string prop
        )
        {
            object value = default(T);
            entity.Data.TryGetValue(
                prop,
                out value
            );
            if (value == null)
            {
                return default(T);
            }
            return (T)value;
        }

        public static void SetProperty<T>(
            this IObjectEntity entity,
            string prop,
            T value
        )
        {
            entity.Data[prop] = value;
        }

        /// <summary>
        /// Will populate the Raw Data into the state data of the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="prop"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T PopulateData<T>(
            this IObjectEntity entity,
            string prop,
            T defaultValue = default(T)
        )
        {
            var rawData = entity.RawData;
            var data = entity.Data;
            if (!rawData.ContainsKey(prop))
            {
                data[prop] = defaultValue;
            }
            // Newtonsoft Type
            else if (rawData[prop] is JObject)
            {
                var tempProp = (JObject)rawData[prop];
                data[prop] = tempProp.ToObject<T>();
            }
            // Newtonsoft Type
            else if (rawData[prop] is JToken)
            {
                var tempProp = (JToken)rawData[prop];
                data[prop] = tempProp.ToObject<T>();
            }
            // .NET JSON Type
            else if (rawData[prop] is JsonElement)
            {
                var tempProp = (JsonElement)rawData[prop];
                data[prop] = tempProp.ToObject<T>(
                    JSON_OPTIONS
                );
            }
            // .NET JSON Type
            else if (rawData[prop] is JsonDocument)
            {
                var tempProp = (JsonDocument)rawData[prop];
                data[prop] = tempProp.ToObject<T>(
                    JSON_OPTIONS
                );
            }
            else if (rawData[prop] is T)
            {
                data[prop] = rawData[prop];
            }
            else
            {
                data[prop] = defaultValue;
            }
            return entity.GetProperty<T>(prop);
        }

        public static Dictionary<string, object> AllData(
            this IObjectEntity entity
        )
        {
            var data = new Dictionary<string, object>();
            foreach (var prop in entity.RawData)
            {
                data[prop.Key] = prop.Value;
            }
            foreach (var prop in entity.Data)
            {
                data[prop.Key] = prop.Value;
            }
            return data;
        }
    }

    internal static partial class JsonExtensions
    {
        internal static T ToObject<T>(
            this JsonElement element,
            JsonSerializerOptions options = null
        )
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(
                bufferWriter
            ))
            {
                element.WriteTo(writer);
            }
            return JsonSerializer.Deserialize<T>(
                bufferWriter.WrittenSpan,
                options
            );
        }

        internal static T ToObject<T>(
            this JsonDocument document,
            JsonSerializerOptions options = null
        )
        {
            if (document == null)
            {
                throw new ArgumentNullException(
                    nameof(document)
                );
            }
            return document.RootElement.ToObject<T>(
                options
            );
        }
    }
}