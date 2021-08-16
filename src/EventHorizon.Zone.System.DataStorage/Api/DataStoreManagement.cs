namespace EventHorizon.Zone.System.DataStorage.Api
{
    using global::System.Collections.Generic;

    public interface DataStoreManagement
    {
        IDictionary<string, object> Data();

        void Set(
            IDictionary<string, object> data
        );

        void Set(
            string key,
            object value
        );

        void Delete(
            string key
        );

        void UpdateSchema(
            string key,
            string type
        );

        void DeleteFromSchema(
            string key
        );
    }
}
