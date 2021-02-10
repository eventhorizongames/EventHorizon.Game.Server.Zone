/// <summary>
/// Id: target_in_range_of_caster
/// var tagList = new string[] { "Type:SkillValidatorScript" };
/// 
/// Caster - 
/// Target - 
/// Data: { min: number; max: number; }
/// Services: { Vector3: Vector3; }
/// </summary>

using System.Collections.Generic;
using System.Numerics;
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

        var caster = data.Get<IObjectEntity>("Caster");
        var target = data.Get<IObjectEntity>("Target");
        var validatorData = data.Get<IDictionary<string, object>>("ValidatorData");

        var min = (long)validatorData["min"];
        var max = (long)validatorData["max"];

        var distance = Vector3.Distance(
            caster.Transform.Position,
            target.Transform.Position
        );

        if (distance >= min && distance <= max)
        {
            return new SkillValidatorResponse
            {
                Success = true
            };
        }

        return new SkillValidatorResponse
        {
            Success = false,
            ErrorCode = "target_not_in_range",
            ErrorMessageTemplateKey = "targetNotInRange"
        };
    }
}
