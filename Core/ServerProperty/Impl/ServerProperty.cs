using System.Linq;
using System.Collections.Concurrent;

namespace EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl
{
    public class ServerProperty : IServerProperty
    {
        private static ConcurrentDictionary<string, object> PROPERTIES = new ConcurrentDictionary<string, object>();

        public T Get<T>(string key)
        {
            object value = null;
            PROPERTIES.TryGetValue(key, out value);
            return (T)value;
        }

        public void Set(string key, object value)
        {
            PROPERTIES.AddOrUpdate(key, value, (currentKey, currentValue) => value);
        }
    }
}