namespace EventHorizon.Zone.Core.Model.Entity
{
    using System;
    using System.Buffers;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text.Json;

    using Newtonsoft.Json.Linq;

    public static class DataPropertyExtensions
    {
        private static readonly JsonSerializerOptions JSON_OPTIONS = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static TProperty? GetProperty<TProperty>(
            this IObjectEntity entity,
            string prop
        )
        {
            if (entity.Data.TryGetValue(
                prop,
                out var value
            ))
            {
                return (TProperty)value;
            }
            return default;
        }

        public static TEntity SetProperty<TProperty, TEntity>(
            this TEntity entity,
            string prop,
            TProperty value
        ) where TEntity : IObjectEntity
        {
            if (value is null)
            {
                entity.Data.Remove(prop, out _);
                return entity;
            }

            entity.Data[prop] = value;
            return entity;
        }

        public static bool ContainsProperty(
            this IObjectEntity entity,
            string prop
        ) => entity.Data.ContainsKey(
            prop
        );

        /// <summary>
        /// Will populate the Raw Data into the state data of the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="prop"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public static TProperty? PopulateData<TProperty>(
            this IObjectEntity entity,
            string prop,
            TProperty? defaultValue = default,
            bool setDefaultValue = true
        )
        {
            var rawData = entity.RawData;
            var data = entity.Data;
            TProperty? value;

            // Is Already Populated
            if (data.ContainsKey(prop))
            {
                // Return already populated prop.
                return entity.GetProperty<TProperty>(prop);
            }
            else if (!rawData.ContainsKey(prop) && !setDefaultValue)
            {
                return defaultValue;
            }
            else if (!rawData.ContainsKey(prop))
            {
                value = defaultValue;
            }
            // Newtonsoft Type
            else if (rawData[prop] is JObject jObjectProp)
            {
                value = jObjectProp.ToObject<TProperty>();
            }
            // Newtonsoft Type
            else if (rawData[prop] is JToken jTokenProp)
            {
                value = jTokenProp.ToObject<TProperty>();
            }
            // .NET JSON Type
            else if (rawData[prop] is JsonElement jsonElementProp)
            {
                value = jsonElementProp.ToObject<TProperty>(
                    JSON_OPTIONS
                );
            }
            // .NET JSON Type
            else if (rawData[prop] is JsonDocument jsonDocumentProp)
            {
                value = jsonDocumentProp.ToObject<TProperty>(
                    JSON_OPTIONS
                );
            }
            else if (rawData[prop] is TProperty rawDataProp)
            {
                value = rawDataProp;
            }
            else
            {
                value = defaultValue;
            }

            if (value is null)
            {
                data.Remove(prop, out _);

                return default;
            }

            data[prop] = value;
            return entity.GetProperty<TProperty>(prop);
        }

        public static ConcurrentDictionary<string, object> AllData(
            this IObjectEntity entity
        )
        {
            var data = new ConcurrentDictionary<string, object>();
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
        internal static T? ToObject<T>(
            this JsonElement element,
            JsonSerializerOptions? options = null
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

        internal static T? ToObject<T>(
            this JsonDocument document,
            JsonSerializerOptions? options = null
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
