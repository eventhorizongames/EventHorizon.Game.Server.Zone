/// <summary>
/// Id: caster_has_cost
/// var tagList = new string[] { "Type:SkillValidatorScript" };
/// 
/// Caster - 
/// Target - 
/// Data: { propertyName: string; valueProperty: string; cost: number; }
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

        var caster = data.Get<IObjectEntity>("Caster");
        var validatorData = data.Get<IDictionary<string, object>>("ValidatorData");

        var propertyName = (string)validatorData["propertyName"];
        var valueProperty = (string)validatorData["valueProperty"];
        var cost = (long)validatorData["cost"];

        var casterPropertyValue = caster.GetProperty<dynamic>(propertyName)[valueProperty];

        if (casterPropertyValue >= cost)
        {
            return new SkillValidatorResponse
            {
                Success = true
            };
        }

        return new SkillValidatorResponse
        {
            Success = false,
            ErrorCode = "caster_not_enough_points",
            ErrorMessageTemplateKey = "casterNotEnoughPoints",
            ErrorMessageTemplateData = new
            {
                Cost = cost,
                ValueProperty = valueProperty
            }
        };
    }
}
