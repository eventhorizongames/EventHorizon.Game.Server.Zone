/// <summary>
/// Id: target_in_line_of_sight_from_caster
/// var tagList = new string[] { "Type:SkillValidatorScript" };
/// 
/// Caster - 
/// Target - 
/// Data: { }
/// </summary>

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

        // TODO: Implement this, if feel like it.
        return new SkillValidatorResponse
        {
            Success = true
        };
    }
}
