/// <summary>
/// Id: validate_success
/// var tagList = new string[] { "Type:SkillValidatorScript" };
/// 
/// Caster - 
/// Target - 
/// Data: { percent: number; messageTemplate: string; }
/// Services: { Random: IRandomNumberGenerator; }
/// </summary>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;


using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        var validatorData = data.Get<IDictionary<string, object>>("ValidatorData");

        var precent = (long)validatorData["percent"];
        var errorMessageTemplateKey = (string)validatorData["errorMessageTemplateKey"];
        var randomNumber = services.Random.Next(100);

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
    }
}
