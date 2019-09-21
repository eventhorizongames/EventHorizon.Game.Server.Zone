using System.Collections.Generic;

namespace EventHorizon.Zone.System.Combat.Skill.Model
{
    public struct SkillValidatorResponse
    {
        public string Validator { get; set; }
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessageTemplateKey { get; set; }
        public object ErrorMessageTemplateData { get; set; }
    }
}