namespace EventHorizon.Zone.System.Server.Scripts.Actions
{
    using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
    using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

    public static class ServerScriptsSystemReloadedAdminClientActionToAllEvent
    {
        public static AdminClientActionGenericToAllEvent Create() => new(
            "SERVER_SCRIPTS_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
            new ServerScriptsSystemReloadedClientActionToAllEventData()
        );

        private class ServerScriptsSystemReloadedClientActionToAllEventData
            : IAdminClientActionData
        {

        }
    }
}
