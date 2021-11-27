namespace EventHorizon.Zone.System.Template.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionTemplateSystemReloadedToAllEvent
{
    public static AdminClientActionGenericToAllEvent Create() => new(
        "TEMPLATE_SYSTEM_RELOADED_ADMIN_CLIENT_ACTION",
        new AdminClientActionTemplateSystemReloadedToAllEventData()
    );

    private class AdminClientActionTemplateSystemReloadedToAllEventData
        : IAdminClientActionData
    {

    }
}
