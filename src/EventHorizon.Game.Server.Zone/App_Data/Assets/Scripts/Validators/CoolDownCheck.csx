/// <summary>
/// Id - cool_down_check
/// 
/// Caster - 
/// Target - 
/// Skill - { id: string; }
/// Data: { message: string; messageCode: string; }
/// Services: { Mediator: IMediator; DateTime: IDateTimeService; }
/// </summary>

var skillState = Caster.GetProperty<dynamic>("skillState");

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
    ErrorCode = "skill_not_ready",
    ErrorMessageTemplateKey = "SkillNotReady",
};