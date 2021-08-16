namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model
{
    using EventHorizon.Zone.System.Server.Scripts.Model;

    public struct SkillValidatorResponse : ServerScriptResponse
    {
        public string Validator { get; set; }
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessageTemplateKey { get; set; }
        public object ErrorMessageTemplateData { get; set; }

        public string Message => string.Empty;
    }
}
