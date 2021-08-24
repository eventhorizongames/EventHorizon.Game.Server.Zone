namespace EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction
{
    using EventHorizon.Zone.Core.Events.Client.Generic;

    public static class ClientActionRunSkillActionToSingleEvent
    {
        public static ClientActionGenericToSingleEvent Create(
            ClientActionRunSkillActionForConnectionEvent data
        ) => new(
            data.ConnectionId,
            "RunSkillAction",
            data
        );
    }
}
