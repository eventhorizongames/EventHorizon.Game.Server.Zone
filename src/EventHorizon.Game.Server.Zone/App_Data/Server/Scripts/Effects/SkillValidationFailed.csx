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

using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var skill = Data.Get<SkillInstance>("Skill");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");
var priorState = Data.Get<IDictionary<string, object>>("PriorState");

var validationResponse = (SkillValidatorResponse)priorState["ValidationResponse"];
var actionData = new
{
    MessageCode = (string)effectData["messageCode"],
    MessageTemplate = Services.I18n.Lookup("default", (string)effectData["messageTemplateKey"]),
    TemplateData = new
    {
        CasterName = caster.Name,
        TargetName = target.Name,
        SkillName = skill.Name,
        SkillFailedReason = Services.I18n.Lookup("default", validationResponse.ErrorMessageTemplateKey),
        ErrorData = validationResponse.ErrorMessageTemplateData
    }
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_MessageClient.js",
    Data = actionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};