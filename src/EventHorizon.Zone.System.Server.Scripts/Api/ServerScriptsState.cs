namespace EventHorizon.Zone.System.Server.Scripts.Api
{
    public interface ServerScriptsState
    {
        string CurrentHash { get; }
        void UpdateHash(
            string hash
        );
    }
}
