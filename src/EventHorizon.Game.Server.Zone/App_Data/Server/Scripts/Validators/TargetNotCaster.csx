/// <summary>
/// Id: target_not_caster
/// 
/// Caster: { Id: string; } 
/// Target: { Id: string; }
/// Data: { }
/// </summary>

if (Target.Id != Caster.Id)
{
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false,
    ErrorCode = "caster_can_not_be_target",
    ErrorMessageTemplateKey = "casterCanNotTargetSelf"
};