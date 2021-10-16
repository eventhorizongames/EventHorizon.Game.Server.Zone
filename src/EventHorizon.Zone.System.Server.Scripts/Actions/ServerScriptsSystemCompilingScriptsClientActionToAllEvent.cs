namespace EventHorizon.Zone.System.Server.Scripts.Actions
{
    using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
    using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

    public static class ServerScriptsSystemCompilingScriptsClientActionToAllEvent
    {
        public static AdminClientActionGenericToAllEvent Create() => new(
            "SERVER_SCRIPTS_SYSTEM_COMPILING_SCRIPTS_CLIENT_ACTION_EVENT",
            new ServerScriptsSystemCompilingScriptsClientActionToAllEventData()
        );

        private class ServerScriptsSystemCompilingScriptsClientActionToAllEventData
            : IAdminClientActionData
        {

        }
    }
}
