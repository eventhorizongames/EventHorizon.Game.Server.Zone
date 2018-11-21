/// <summary>
/// Caster - 
/// Target - 
/// Data: { message: string; messageCode: string; }
/// </summary>
var actionData = new
{
    MessageCode = (string)Data["messageCode"],
    MessageTemplate = (string)Data["messageTemplate"],
    TemplateData = new
    {
        CasterName = Caster.Id,
        TargetName = Target.Id,
        Damage = PriorState["Damage"]
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