namespace EventHorizon.Zone.System.Client.Scripts.Actions.Reload;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.Client.Scripts.Model.Client;

public class ClientScriptsAssemblyChangedClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        ClientScriptsAssemblyChangedClientActionData data
    ) => new(
        "CLIENT_SCRIPTS_ASSEMBLY_CHANGED_CLIENT_ACTION_EVENT",
        data
    );
}
