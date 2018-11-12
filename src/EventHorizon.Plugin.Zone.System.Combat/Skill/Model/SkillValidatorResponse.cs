namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillValidatorResponse
    {
        public string Validator { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}