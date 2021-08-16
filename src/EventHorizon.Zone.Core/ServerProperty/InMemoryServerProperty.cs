namespace EventHorizon.Zone.Core.ServerProperty
{
    using System.Collections.Concurrent;

    using EventHorizon.Zone.Core.Model.ServerProperty;

    public class InMemoryServerProperty : IServerProperty
    {
        private ConcurrentDictionary<string, object> PROPERTIES = new ConcurrentDictionary<string, object>();

        public T Get<T>(
            string key
        )
        {
            PROPERTIES.TryGetValue(
                key,
                out var value
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
                (_, __) => value
            );
        }
    }
}
