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
        public const string DATA_STORE_SCHEMA_KEY = "dataStore:Schema";

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

            if (!_map.ContainsKey(
                DATA_STORE_SCHEMA_KEY
            ))
            {
                AddOrUpdate(
                    DATA_STORE_SCHEMA_KEY,
                    new DataStoreSchema()
                );
            }
        }

        public void Set(
            string key,
            object value
        )
        {
            AddOrUpdate(
                key,
                value
            );
        }

        public void Delete(
            string key
        )
        {
            _map.Remove(
                key,
                out _
            );
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
        }

        public void UpdateSchema(
            string key,
            string type
        )
        {
            var metadata = GetCurrentSchema();

            metadata[key] = type;

            AddOrUpdate(
                DATA_STORE_SCHEMA_KEY,
                metadata
            );
        }

        public void DeleteFromSchema(
            string key
        )
        {
            var metadata = GetCurrentSchema();

            metadata.Remove(
                key
            );

            AddOrUpdate(
                DATA_STORE_SCHEMA_KEY,
                metadata
            );
        }

        private DataStoreSchema GetCurrentSchema()
        {
            var metadata = new DataStoreSchema();
            if (TryGetValue<DataStoreSchema>(
                DATA_STORE_SCHEMA_KEY,
                out var value
            ))
            {
                metadata = value;
            }

            return metadata;
        }
    }
}
