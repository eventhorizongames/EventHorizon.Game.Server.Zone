using System.Collections.Generic;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillValidator
    {
        public string Validator { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}