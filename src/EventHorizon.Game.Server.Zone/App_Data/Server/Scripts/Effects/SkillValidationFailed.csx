/// <summary>
/// Effect Id: skill_validation_failed
/// tagList = new string[] { "Type:SkillEffectScript" };
/// 
/// Caster: { Id: long; } 
/// Target: { Id: long; }
/// Skill: { Name: string; }
/// Data: { messageTemplate: string; messageCode: string; }
/// PriorState: { ValidationResponse: ValidationResponse; }
/// Services: { I18n: I18nLookup; }
/// </summary>

using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
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
var skill = data.Get<SkillInstance>("Skill");
var effectData = data.Get<IDictionary<string, object>>("EffectData");
var priorState = data.Get<IDictionary<string, object>>("PriorState");

var validationResponse = ((IList<SkillValidatorResponse>)priorState["ValidationResponse"]).First(a => !a.Success);
var errorCode = (string)effectData["messageTemplateKey"];
var actionData = new
{
    MessageCode = (string)effectData["messageCode"],
    MessageTemplate = services.I18n.Lookup("default", (string)effectData["messageTemplateKey"]),
    TemplateData = new
    {
        CasterName = caster.Name,
        TargetName = target.Name,
        SkillName = skill.Name,
        SkillFailedReason = services.I18n.Lookup("default", validationResponse.ErrorMessageTemplateKey),
        ErrorData = validationResponse.ErrorMessageTemplateData
    }
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_MessageClient.js",
    Data = actionData
};

return SkillEffectScriptResponse
    .New()
    .Add(
        action
    );
    }
}
