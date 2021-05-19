namespace EventHorizon.Zone.System.DataStorage.Model
{
    public interface DataStore
    {
        bool TryGetValue<T>(
            string key,
            out T value
        );

        void AddOrUpdate(
            string key,
            object value
        );
    }
}
