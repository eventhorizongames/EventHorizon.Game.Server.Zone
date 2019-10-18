using System.Collections.Concurrent;
using EventHorizon.Zone.Core.Model.ServerProperty;

namespace EventHorizon.Zone.Core.ServerProperty
{
    public class InMemoryServerProperty : IServerProperty
    {
        private ConcurrentDictionary<string, object> PROPERTIES = new ConcurrentDictionary<string, object>();

        public T Get<T>(
            string key
        )
        {
            object value = null;
            PROPERTIES.TryGetValue(
                key, 
                out value
            );
            return (T)value;
        }

        public void Set(
            string key, 
            object value
        )
        {
            PROPERTIES.AddOrUpdate(
                key, 
                value, 
                (currentKey, currentValue) => value
            );
        }
    }
}