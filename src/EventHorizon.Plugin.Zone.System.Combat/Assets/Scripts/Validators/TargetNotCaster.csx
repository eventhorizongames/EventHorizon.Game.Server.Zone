/// <summary>
/// Id: target_not_caster
/// 
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
    ErrorMessage = $"(${{targetName}}) can not target self",
    Success = false
};