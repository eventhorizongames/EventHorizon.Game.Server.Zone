namespace EventHorizon.Zone.System.Player.Client;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.Player.Model.Client;

public static class ClientActionPlayerSystemReloadedToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        PlayerSystemReloadedEventData data
    ) => new(
        "Player.PLAYER_SYSTEM_RELOADED",
        data
    );
}
