using System.Collections.Generic;

namespace EventHorizon.Zone.System.Combat.Skill.Model
{
    public struct SkillValidator
    {
        public string Validator { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}