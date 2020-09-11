using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");

var animation = (string)effectData["animation"];

var actionData = new
{
    EntityId = caster.Id,
    DirectionEntityId = target.Id,
    Animation = animation
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_StartEntityDirectionOffsetAnimation.js",
    Data = actionData
};

return SkillEffectScriptResponse
    .New()
    .Add(
        action
    );