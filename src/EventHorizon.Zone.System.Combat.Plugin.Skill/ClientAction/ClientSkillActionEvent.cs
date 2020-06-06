namespace EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction
{
    using EventHorizon.Zone.Core.Model.Client;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

    public struct ClientSkillActionEvent : ClientSkillAction, IClientActionData
    {
        public string Action { get; set; }
        public object Data { get; set; }
    }
}