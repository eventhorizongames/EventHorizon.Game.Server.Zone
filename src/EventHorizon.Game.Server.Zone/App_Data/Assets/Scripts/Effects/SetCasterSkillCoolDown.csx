/// <summary>
/// Effect Id: set_caster_skill_cool_down
/// 
/// Data: { Skill: SkillInstance; }
/// Services: { Mediator: IMediator; DateTime: IDateTimeService; }
/// PriorState: {Code: string; ValidationMessage: string; Skill: SkillInstance; }
/// </summary>
/// <returns></returns>
var coolDown = (long)Data["coolDown"];

var skill = ((SkillInstance)PriorState["Skill"]);
var casterSkillState = Caster.GetProperty<dynamic>("SkillState");
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