/// <summary>
/// Id: validate_success
/// 
/// Caster - 
/// Target - 
/// Data: { percent: number; messageTemplate: string; }
/// Services: { Random: IRandomNumberGenerator; }
/// </summary>
/// 
var precent = (long)Data["percent"];
var randomNumber = Services.Random.Next(100);

if (randomNumber <= precent)
{
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false,
    ErrorCode = "skill_validation_failed"
};