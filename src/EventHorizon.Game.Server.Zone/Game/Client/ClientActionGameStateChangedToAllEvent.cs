namespace EventHorizon.Game.Server.Zone.Game.Client
{
    using EventHorizon.Game.Server.Zone.Game.Model.Client;
    using EventHorizon.Zone.Core.Events.Client.Generic;

    public static class ClientActionGameStateChangedToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            GameStateChangedData data
        ) => new ClientActionGenericToAllEvent(
            "CLIENT_ACTION_GAME_STATE_UPDATED",
            data
        );
    }
}