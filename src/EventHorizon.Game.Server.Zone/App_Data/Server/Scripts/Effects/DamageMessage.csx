/// <summary>
/// Effect Id: damage_message
/// 
/// Caster - 
/// Target - 
/// PriorState: { Damage: long; }
/// Data: { message: string; messageCode: string; messageTemplateKey: string; }
/// Services: { I18n: I18nLookup; }
/// </summary>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");
var priorState = Data.Get<IDictionary<string, object>>("PriorState");

var actionData = new
{
    MessageCode = (string)effectData["messageCode"],
    MessageTemplate = Services.I18n.Lookup("default", (string)effectData["messageTemplateKey"]),
    TemplateData = new
    {
        CasterName = caster.Name,
        TargetName = target.Name,
        Damage = priorState["Damage"]
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