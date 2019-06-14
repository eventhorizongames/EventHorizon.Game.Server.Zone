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

var coolDown = (long)Data["coolDown"];

var casterSkillState = Caster.GetProperty<dynamic>("skillState");
var skillState = casterSkillState.SkillList[Skill.Id];

skillState.CooldownFinishes = Services.DateTime.Now
    .AddMilliseconds(
        coolDown
    );
casterSkillState.SkillList[Skill.Id] = skillState;

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>()
};