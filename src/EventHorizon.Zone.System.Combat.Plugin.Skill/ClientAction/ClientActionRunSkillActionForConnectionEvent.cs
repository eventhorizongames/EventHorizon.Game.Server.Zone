namespace EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction
{
    using EventHorizon.Zone.Core.Model.Client;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

    public struct ClientActionRunSkillActionForConnectionEvent : ClientSkillAction, IClientActionData
    {
        public string ConnectionId { get; }
        public string Action { get; }
        public object Data { get; }

        public ClientActionRunSkillActionForConnectionEvent(
            string connectionId,
            string action,
            object data
        )
        {
            ConnectionId = connectionId;
            Action = action;
            Data = data;
        }
    }
}
