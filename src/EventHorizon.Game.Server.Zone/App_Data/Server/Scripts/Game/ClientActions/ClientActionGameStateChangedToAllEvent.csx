using ClientEvents = EventHorizon.Zone.Core.Events.Client.Generic;

public static class ClientActionGameStateChangedToAllEvent
{
    public static ClientEvents.ClientActionGenericToAllEvent Create(GameStateChangedData data) =>
        new("CLIENT_ACTION_GAME_STATE_UPDATED", data);
}
