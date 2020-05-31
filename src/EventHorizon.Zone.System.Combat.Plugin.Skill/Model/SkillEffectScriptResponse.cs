namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
    using EventHorizon.Zone.System.Server.Scripts.Model;

    public struct SkillEffectScriptResponse : ServerScriptResponse
    {
        public IDictionary<string, object> State { get; set; }
        public List<ClientSkillActionEvent> ActionList { get; set; }

        public bool Success => true;
        public string Message => string.Empty;
    }
}