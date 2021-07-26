namespace System.Collections.Generic
{
    using Newtonsoft.Json.Linq;

    public static class DictionaryExtensions
    {
        public static T GetValueOrDefault<T>(
            this IDictionary<string, object> data,
            string prop,
            T defaultValue
        ) => data.TryGetValue(
                prop,
                out var propValue
            ) ? CastValue<T>(
                propValue
            ) : defaultValue;

        /// <summary>
        /// This is used to help with the casting of objects Deserialize by Newtonsoft.Json 
        /// </summary>
        /// <param name="propValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T CastValue<T>(
            object propValue
        )
        {
            if (propValue is JObject)
            {
                return ((JObject)propValue).ToObject<T>();
            }
            return (T)propValue;
        }
    }
}