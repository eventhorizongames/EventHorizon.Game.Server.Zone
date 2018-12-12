/// <summary>
/// Effect Id: damage_message
/// 
/// Caster - 
/// Target - 
/// PriorState: { Damage: long; }
/// Data: { message: string; messageCode: string; messageTemplateKey: string; }
/// Services: { I18n: I18nLookup; }
/// </summary>

var actionData = new
{
    MessageCode = (string)Data["messageCode"],
    MessageTemplate = Services.I18n.Lookup("default", (string)Data["messageTemplateKey"]),
    TemplateData = new
    {
        CasterName = Caster.Name,
        TargetName = Target.Name,
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