namespace EventHorizon.Zone.System.Server.Scripts.StateManagement
{
    using EventHorizon.Zone.System.Server.Scripts.Model.State;
    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Text.Json;

    public class KeyValueServerScriptRuntimeState
        : ServerScriptRuntimeState
    {
        private readonly ConcurrentDictionary<string, object> _map = new();

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
    }
}
