namespace EventHorizon.Zone.Core.ServerProperty
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.ServerProperty;

    public class InMemoryServerProperty
        : IServerProperty
    {
        private readonly ConcurrentDictionary<string, object> _properties = new();

        public T? Get<T>(
            string key
        )
        {
            _properties.TryGetValue(
                key,
                out var value
            );
            return (T?)value;
        }

        public void Set(
            string key,
            object value
        )
        {
            _properties.AddOrUpdate(
                key,
                value,
                (_, _) => value
            );
        }

        public void Remove(
            string key
        )
        {
            _properties.Remove(
                key,
                 out _
             );
        }
    }
}
