namespace EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction
{
    using EventHorizon.Zone.Core.Model.Client;

    public struct ClientSkillActionEvent : IClientActionData
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }
}