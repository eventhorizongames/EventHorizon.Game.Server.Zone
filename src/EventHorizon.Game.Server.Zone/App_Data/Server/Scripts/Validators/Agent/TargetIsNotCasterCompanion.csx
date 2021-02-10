/// <summary>
/// Id: target_is_not_caster_companion
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

        var caster = data.Get<IObjectEntity>("Caster");
        var target = data.Get<IObjectEntity>("Target");

        var targetOwnerId = target.GetProperty<dynamic>("ownerState")["ownerId"];

        if (targetOwnerId == caster.GlobalId)
        {
            return new SkillValidatorResponse
            {
                Success = false,
                ErrorCode = "target_is_casters_companion",
                ErrorMessageTemplateKey = "targetIsCastersCompanion",
            };
        }
        return new SkillValidatorResponse
        {
            Success = true
        };
    }
}
