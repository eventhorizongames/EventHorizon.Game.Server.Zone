namespace EventHorizon.Zone.System.ServerModule.Actions.Reload;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.ServerModule.Model.Client;

public class SendServerModuleSystemReloadedClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        ServerModuleSystemReloadedClientActionData data
    ) => new(
        "SERVER_MODULE_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
        data
    );
}
