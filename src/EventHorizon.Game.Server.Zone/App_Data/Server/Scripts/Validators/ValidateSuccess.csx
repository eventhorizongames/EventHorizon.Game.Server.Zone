/// <summary>
/// Id: validate_success
/// 
/// Caster - 
/// Target - 
/// Data: { percent: number; messageTemplate: string; }
/// Services: { Random: IRandomNumberGenerator; }
/// </summary>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

var validatorData = Data.Get<IDictionary<string, object>>("ValidatorData");

var precent = (long)validatorData["percent"];
var errorMessageTemplateKey = (string)validatorData["errorMessageTemplateKey"];
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
    ErrorCode = "skill_validation_failed",
    ErrorMessageTemplateKey = errorMessageTemplateKey ?? "validationFailedMessage",
};