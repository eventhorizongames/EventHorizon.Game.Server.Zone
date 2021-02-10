/// <summary>
/// Effect Id: set_caster_skill_cool_down
/// tagList = new string[] { "Type:SkillEffectScript" };
/// 
/// Caster: { SkillState: ISkillState; }
/// Target: 
/// Skill: { Id: string; }
/// Services: { Mediator: IMediator; DateTime: IDateTimeService; }
/// Data: { coolDown: long; }
/// PriorState: -
/// </summary>
/// <returns></returns>
/// 
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;


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

var coolDown = (long)effectData["coolDown"];

var casterSkillState = caster.GetProperty<SkillState>("skillState");
var skillMap = casterSkillState.SkillMap;
var skillDetails = casterSkillState.SkillMap.Get(
    skill.Id
);

skillDetails.CooldownFinishes = services.DateTime.Now
    .AddMilliseconds(
        coolDown
    );
skillMap = skillMap.Set(
    skillDetails
);
casterSkillState.SkillMap = skillMap;
caster.SetProperty(
    SkillState.PROPERTY_NAME,
    casterSkillState
);


return SkillEffectScriptResponse.New();
    }
}
