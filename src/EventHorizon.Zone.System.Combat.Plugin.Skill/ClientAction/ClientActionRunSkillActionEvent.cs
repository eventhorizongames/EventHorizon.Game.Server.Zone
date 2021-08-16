namespace EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction
{
    using EventHorizon.Zone.Core.Events.Client.Generic;

    public static class ClientActionRunSkillActionEvent
    {
        public static ClientActionGenericToAllEvent Create(
            ClientSkillActionEvent data
        ) => new ClientActionGenericToAllEvent(
            "RunSkillAction",
            data
        );
    }
}
