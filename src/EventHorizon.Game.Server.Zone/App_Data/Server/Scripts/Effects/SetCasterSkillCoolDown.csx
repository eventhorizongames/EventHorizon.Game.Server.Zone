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
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var skill = Data.Get<SkillInstance>("Skill");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");

var coolDown = (long)effectData["coolDown"];

var casterSkillState = caster.GetProperty<dynamic>("skillState");
var skillState = casterSkillState.SkillList[skill.Id];

skillState.CooldownFinishes = Services.DateTime.Now
    .AddMilliseconds(
        coolDown
    );
casterSkillState.SkillList[skill.Id] = skillState;

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>()
};