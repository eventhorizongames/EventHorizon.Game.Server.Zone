namespace EventHorizon.Zone.System.Server.Scripts.State
{
    using EventHorizon.Zone.System.Server.Scripts.Api;

    public class StandardServerScriptsState
        : ServerScriptsState
    {
        public string CurrentHash { get; private set; } = string.Empty;

        public void UpdateHash(
            string hash
        )
        {
            CurrentHash = hash;
        }
    }
}
