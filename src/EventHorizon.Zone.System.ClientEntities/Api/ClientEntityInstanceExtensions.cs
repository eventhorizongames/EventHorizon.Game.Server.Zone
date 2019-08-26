using Newtonsoft.Json.Linq;

namespace EventHorizon.Zone.System.ClientEntities.Api
{
    public static class ClientEntityInstanceExtensions
    {
        public static T GetProperty<T>(
            this IClientEntityInstance entity,
            string property
        )
        {
            if (!entity.Properties.ContainsKey(
                property
            ))
            {
                return default(T);
            }
            else if (entity.Properties[property] is JObject)
            {
                return ((JObject)entity.Properties[property]).ToObject<T>();
            }
            else if (entity.Properties[property] is JToken)
            {
                return ((JToken)entity.Properties[property]).ToObject<T>();
            }
            return (T)entity.Properties[property];
        }
    }
}