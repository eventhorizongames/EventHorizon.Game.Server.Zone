namespace EventHorizon.Zone.System.Template.ClientActions;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.Core.Model.Client;

public static class ClientActionTemplateSystemReloadedToAllEvent
{
    public static ClientActionGenericToAllEvent Create() => new(
        "TEMPLATE_SYSTEM_RELOADED_CLIENT_ACTION",
        new ClientActionTemplateSystemReloadedToAllEventData()
    );

    private class ClientActionTemplateSystemReloadedToAllEventData
        : IClientActionData
    {

    }
}
