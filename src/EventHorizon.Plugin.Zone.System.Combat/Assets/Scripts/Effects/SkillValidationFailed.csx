/// <summary>
/// Effect Id: skill_validation_failed
/// 
/// Caster: { Id: string; } 
/// Target: { Id: string; }
/// Skill: { Name: string; }
/// Data: { messageTemplate: string; messageCode: string; }
/// PriorState: { ValidationMessage: string; }
/// </summary>

var actionData = new
{
    MessageCode = (string)Data["messageCode"],
    MessageTemplate = (string)Data["messageTemplate"],
    TemplateData = new
    {
        CasterName = Caster.Id,
        TargetName = Target.Id,
        SkillName = Skill.Name,
        SkillFailedReason = PriorState["ValidationMessage"]
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