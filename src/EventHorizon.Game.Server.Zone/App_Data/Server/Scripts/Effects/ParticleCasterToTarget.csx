using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");

var actionData = new
{
    StartEntityId = caster.Id,
    EndingEntityId = target.Id,
    ParticleTemplateId = (string)effectData["particleTemplateId"],
    Duration = (long)effectData["duration"],
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_ParticleToEntity.js",
    Data = actionData
};

return SkillEffectScriptResponse
    .New()
    .Add(
        action
    );