/// <summary>
/// Effect Id: set_caster_skill_cool_down
/// 
/// Caster: { SkillState: ISkillState; }
/// Target: 
/// Skill: { Id: string; }
/// Services: { Mediator: IMediator; DateTime: IDateTimeService; }
/// Data: { coolDown: long; }
/// PriorState: -
/// </summary>
/// <returns></returns>
/// 
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var skill = Data.Get<SkillInstance>("Skill");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");

var coolDown = (long)effectData["coolDown"];

var casterSkillState = caster.GetProperty<SkillState>("skillState");
var skillDetails = casterSkillState.SkillMap.Get(
    skill.Id
);

skillDetails.CooldownFinishes = Services.DateTime.Now
    .AddMilliseconds(
        coolDown
    );
casterSkillState.SkillMap.Set(
    skillDetails
);


return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>()
};