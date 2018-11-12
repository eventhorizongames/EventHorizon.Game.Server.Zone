/// <summary>
/// Caster - 
/// Target - 
/// Data: {no data}
/// </summary>

if (Target.Id != Caster.Id) {
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false
};