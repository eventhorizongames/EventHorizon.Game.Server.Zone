using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace EventHorizon.Game.Server.Zone.Model.Entity
{
    public static class DataPropertyExtensions
    {
        public static T GetProperty<T>(this IObjectEntity entity, string prop)
        {
            object value = default(T);
            entity.Data.TryGetValue(prop, out value);
            if (value == null)
            {
                return default(T);
            }
            return (T)value;
        }

        public static void SetProperty<T>(this IObjectEntity entity, string prop, T value)
        {
            entity.Data[prop] = value;
        }

        /// <summary>
        /// TODO: Add default value to be used when temp data is not found.
        /// </summary>
        /// <param name="prop"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T PopulateData<T>(this IObjectEntity entity, string prop)
        {
            var rawData = entity.RawData;
            var data = entity.Data;
            if (!rawData.ContainsKey(prop))
            {
                data[prop] = default(T);
            }
            else if (rawData[prop] is JObject)
            {
                var tempProp = (JObject)rawData[prop];
                data[prop] = tempProp.ToObject<T>();
            }
            else if (rawData[prop] is JToken)
            {
                var tempProp = (JToken)rawData[prop];
                data[prop] = tempProp.ToObject<T>();
            }
            else if (rawData[prop] is T)
            {
                data[prop] = rawData[prop];
            }
            else
            {
                data[prop] = default(T);
            }
            return entity.GetProperty<T>(prop);
        }

        public static Dictionary<string, object> AllData(this IObjectEntity entity)
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
}