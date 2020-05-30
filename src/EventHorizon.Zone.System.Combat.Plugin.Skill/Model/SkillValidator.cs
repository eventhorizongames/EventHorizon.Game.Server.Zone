using System.Collections.Generic;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model
{
    public struct SkillValidator
    {
        public string Validator { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}