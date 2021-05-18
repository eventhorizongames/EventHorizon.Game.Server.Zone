namespace EventHorizon.Zone.System.Server.Scripts.Model.State
{
    public interface ServerScriptRuntimeState
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
