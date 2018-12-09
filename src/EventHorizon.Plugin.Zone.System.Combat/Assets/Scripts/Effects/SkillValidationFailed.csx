/// <summary>
/// Effect Id: skill_validation_failed
/// 
/// Caster: { Id: long; } 
/// Target: { Id: long; }
/// Skill: { Name: string; }
/// Data: { messageTemplate: string; messageCode: string; }
/// PriorState: { ValidationResponse: ValidationResponse; }
/// Services: { I18n: I18nLookup; }
/// </summary>

var validationResponse = (SkillValidatorResponse)PriorState["ValidationResponse"];
var actionData = new
{
    MessageCode = (string)Data["messageCode"],
    MessageTemplate = Services.I18n.Lookup("default", (string)Data["messageTemplateKey"]),
    TemplateData = new
    {
        CasterName = Caster.Id,
        TargetName = Target.Id,
        SkillName = Skill.Name,
        SkillFailedReason = Services.I18n.Lookup("default", validationResponse.ErrorMessageTemplateKey),
        ErrorData = validationResponse.ErrorMessageTemplateData
    }
};
var action = new ClientSkillActionEvent
{
    Action = "message_client",
    Data = actionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};