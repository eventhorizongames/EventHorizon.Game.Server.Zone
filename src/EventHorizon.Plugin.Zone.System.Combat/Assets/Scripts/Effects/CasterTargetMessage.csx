/// <summary>
/// Effect Id: caster_target_message
/// 
/// Caster: 
/// - Id: long
/// Target: 
/// - Id: long
/// Data: 
/// - messageCode: string
/// - messageTemplateKey: string
/// Services: 
/// - I18n: I18nLookup
/// </summary>

var actionData = new
{
    MessageCode = (string)Data["messageCode"],
    MessageTemplate = Services.I18n.Lookup("default", (string)Data["messageTemplateKey"]),
    TemplateData = new
    {
        CasterName = Caster.Id,
        TargetName = Target.Id
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