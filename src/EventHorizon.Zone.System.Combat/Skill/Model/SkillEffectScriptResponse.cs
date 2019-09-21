using System.Collections.Generic;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;

namespace EventHorizon.Zone.System.Combat.Skill.Model
{
    public struct SkillEffectScriptResponse
    {
        public IDictionary<string, object> State { get; set; }
        public List<ClientSkillActionEvent> ActionList { get; set; }
    }
}