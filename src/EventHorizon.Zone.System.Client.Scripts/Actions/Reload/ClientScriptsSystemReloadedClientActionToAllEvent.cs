namespace EventHorizon.Zone.System.Client.Scripts.Actions.Reload;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.Client.Scripts.Model.Client;

public class ClientScriptsSystemReloadedClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        ClientScriptsSystemReloadedClientActionData data
    ) => new(
        "CLIENT_SCRIPTS_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
        data
    );
}
