namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Server.Scripts.Model;

    public struct SkillEffectScriptResponse : ServerScriptResponse
    {
        public IDictionary<string, object> State { get; set; }
        public List<ClientSkillAction> ActionList { get; set; }

        public bool Success => true;
        public string Message => string.Empty;

        public SkillEffectScriptResponse Add(
            ClientSkillAction action
        )
        {
            if (ActionList == null)
            {
                ActionList = new List<ClientSkillAction>();
            }
            ActionList.Add(
                action
            );
            return this;
        }

        public SkillEffectScriptResponse Set(
            string key,
            object value
        )
        {
            if (State == null)
            {
                State = new Dictionary<string, object>();
            }
            State[key] = value;
            return this;
        }

        public static SkillEffectScriptResponse New()
            => new SkillEffectScriptResponse
            {
                State = new Dictionary<string, object>(),
                ActionList = new List<ClientSkillAction>(),
            };
    }
}