using System.Collections.Generic;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillEffect
    {
        public string Comment { get; set; }
        public string Effect { get; set; }
        public string Target { get; set; }
        public string DurationType { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public SkillValidator[] Validator { get; set; }
        public SkillModifier[] Modifier { get; set; }
        public SkillEffect[] Next { get; set; }
    }
}