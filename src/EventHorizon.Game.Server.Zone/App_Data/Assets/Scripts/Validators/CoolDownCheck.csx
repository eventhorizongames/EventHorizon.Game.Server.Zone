/// <summary>
/// Id - cool_down_check
/// 
/// Caster - 
/// Target - 
/// Skill - { id: string; }
/// Services: { Mediator: IMediator; DateTime: IDateTimeService; }
/// Data: { message: string; messageCode: string; }
/// </summary>

var skillState = Caster.GetProperty<dynamic>("SkillState");

var skillReady = Services.DateTime.Now > skillState
                .SkillList[Skill.Id]
                .CooldownFinishes;

if (skillReady)
{
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false,
    ErrorMessage = "${skillName} is not ready.",
};