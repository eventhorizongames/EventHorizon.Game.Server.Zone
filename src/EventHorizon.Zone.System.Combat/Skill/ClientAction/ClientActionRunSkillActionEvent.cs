namespace EventHorizon.Zone.System.Combat.Skill.ClientAction
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