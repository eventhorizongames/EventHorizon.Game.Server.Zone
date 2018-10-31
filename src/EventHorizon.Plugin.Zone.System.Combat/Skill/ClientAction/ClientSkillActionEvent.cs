using System.Collections.Generic;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction
{
    public struct ClientSkillActionEvent
    {
        public long Duration { get; set; }
        public long Delay { get; set; }
        public string Action { get; set; }
        public object Data { get; set; }
        public List<ClientSkillActionEvent> Next { get; set; }
    }
}