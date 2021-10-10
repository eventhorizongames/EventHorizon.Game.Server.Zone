namespace EventHorizon.Zone.System.Player.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;
    using EventHorizon.Zone.Core.Model.Entity;

    public class PlayerSystemReloadedEventData
        : IClientActionData
    {
        public ObjectEntityConfiguration PlayerConfiguration { get; }

        public PlayerSystemReloadedEventData(
            ObjectEntityConfiguration playerConfiguration
        )
        {
            PlayerConfiguration = playerConfiguration;
        }
    }
}
