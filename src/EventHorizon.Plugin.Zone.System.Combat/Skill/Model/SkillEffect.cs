using System.Collections.Generic;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillEffect
    {
        public string Comment { get; set; }
        public string Effect { get; set; }
        public long Duration { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public SkillValidator[] ValidatorList { get; set; }
        public SkillEffect[] Next { get; set; }
        public SkillEffect[] FailedList { get; set; }
    }
}