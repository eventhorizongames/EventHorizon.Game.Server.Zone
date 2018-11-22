/// <summary>
/// Caster - 
/// Target - 
/// Skill??? Should I just add this?
/// Data: { message: string; messageCode: string; }
/// PriorState: {Code: string; ValidationMessage: string; Skill: SkillInstance; }
/// </summary>

var skill = ((SkillInstance)PriorState["Skill"]);
var actionData = new
{
    MessageCode = (string)Data["messageCode"],
    MessageTemplate = (string)Data["messageTemplate"],
    TemplateData = new
    {
        CasterName = Caster.Id,
        TargetName = Target.Id,
        SkillName = skill.Name,
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