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
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var priorState = Data.Get<IDictionary<string, object>>("PriorState");

var actionData = new
{
    MessageKey = priorState["game:MessageKey"],
    MessageData = new { }
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_DisplayClientMessage.js",
    Data = actionData,
};

return SkillEffectScriptResponse
    .New()
    .Add(
        action
    );