namespace EventHorizon.Zone.System.DataStorage.Provider
{
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Model;
    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Text.Json;

    public class StandardDataStoreProvider
        : DataStore,
        DataStoreManagement
    {
        private readonly ConcurrentDictionary<string, object> _map = new();

        public IDictionary<string, object> Data()
        {
            return _map;
        }

        public void Set(
            IDictionary<string, object> data
        )
        {
            foreach (var item in data)
            {
                AddOrUpdate(
                    item.Key,
                    item.Value
                );
            }
        }

        public void AddOrUpdate(
            string key,
            object value
        )
        {
            _map.AddOrUpdate(
                key,
                value,
                (_, __) => value
            );
        }

        public bool TryGetValue<T>(
            string key,
            out T value
        )
        {
            value = default;

            if (_map.TryGetValue(
               key,
               out var existingValue
            ))
            {
                try
                {
                    value = existingValue.To<T>();
                }
                catch (InvalidCastException)
                {
                    value = TryAndFixCastException<T>(
                        existingValue
                    );
                    if (value == null
                        || value.Equals(default(T))
                    )
                    {
                        return false;
                    }
                    AddOrUpdate(
                        key,
                        value
                    );
                }
                return true;
            }

            return false;
        }

        private static T TryAndFixCastException<T>(
            object existingValue
        )
        {
            try
            {
                return JsonSerializer.Deserialize<T>(
                    JsonSerializer.Serialize(
                        existingValue
                    )
                );
            }
            catch (JsonException)
            {
                return default;
            }
        }    }
}
