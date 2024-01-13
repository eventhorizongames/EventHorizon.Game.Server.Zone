namespace EventHorizon.Zone.System.Editor.Actions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class EditorSystemReloadedAdminClientActionToAllEvent
{
    public static AdminClientActionGenericToAllEvent Create() => new(
        "EDITOR_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
        new EditorSystemReloadedClientActionToAllEventData()
    );

    private class EditorSystemReloadedClientActionToAllEventData
        : IAdminClientActionData
    {

    }
}
