namespace EventHorizon.Zone.System.Server.Scripts.Actions
{
    using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
    using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

    public class ServerScriptsSystemFinishedScriptsCompileClientActionToAllEvent
    {
        public static AdminClientActionGenericToAllEvent Create() => new(
            "SERVER_SCRIPTS_SYSTEM_FINISHED_SCRIPTS_COMPILE_CLIENT_ACTION_EVENT",
            new ServerScriptsSystemFinishedScriptsCompileClientActionToAllEventData()
        );

        private class ServerScriptsSystemFinishedScriptsCompileClientActionToAllEventData
            : IAdminClientActionData
        {

        }
    }
}
