/// <summary>
/// Id: target_is_able_to_be_captured
/// var tagList = new string[] { "Type:SkillValidatorScript" };
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// TargetPosition: Vector3
/// PriorState: { }
/// Data: { }
/// </summary>

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

        var target = data.Get<IObjectEntity>("Target");

        var canBeCaptured = target.GetProperty<dynamic>("ownerState")["canBeCaptured"];

        if (!canBeCaptured)
        {
            return new SkillValidatorResponse
            {
                Success = false,
                ErrorCode = "target_is_not_able_to_be_captured",
                ErrorMessageTemplateKey = "targetIsNotAbleToBeCaptured",
            };
        }
        return new SkillValidatorResponse
        {
            Success = true
        };
    }
}
