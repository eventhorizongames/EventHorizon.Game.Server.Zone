namespace EventHorizon.Zone.System.DataStorage.Api
{
    using global::System.Collections.Generic;

    public interface DataStoreManagement
    {
        public IDictionary<string, object> Data();
        public void Set(
            IDictionary<string, object> data
        );
    }
}
