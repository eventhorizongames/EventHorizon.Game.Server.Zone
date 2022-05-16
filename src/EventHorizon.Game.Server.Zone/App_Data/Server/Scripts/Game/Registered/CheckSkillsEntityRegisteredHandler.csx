using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using EntityModel = EventHorizon.Zone.Core.Model.Entity;
using EntityRegisterEvents = EventHorizon.Zone.Core.Events.Entity.Register;
using Logging = Microsoft.Extensions.Logging;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;
using SkillModel = EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

public class __SCRIPT__
    : ServerScriptsModel.ServerScript,
      EntityRegisterEvents.EntityRegisteredEventObserver
{
    public string Id => "__SCRIPT__";
    public Collections.IEnumerable<string> Tags => new Collections.List<string> { };

    private ServerScriptsModel.ServerScriptServices _services;
    private Logging.ILogger _logger;

    public async Task<ServerScriptResponse> Run(
        ServerScriptsModel.ServerScriptServices services,
        ServerScriptsModel.ServerScriptData data
    )
    {
        _services = services;
        _logger = services.Logger<__SCRIPT__>();
        _logger.LogDebug("__SCRIPT__ - Server Script");

        return new ServerScriptsModel.StandardServerScriptResponse(true, "observer_setup");
    }

    public async Task Handle(EntityRegisterEvents.EntityRegisteredEvent args)
    {
        var entity = args.Entity;
        if (entity.Type != EntityModel.EntityType.PLAYER)
        {
            return;
        }

        var skillState = entity.GetProperty<SkillModel.SkillState>(
            SkillModel.SkillState.PROPERTY_NAME
        );
        if (!skillState.SkillMap.Contains(SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID))
        {
            skillState = skillState.SetSkill(
                new SkillModel.SkillStateDetails
                {
                    Id = SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID,
                }
            );
            entity.SetProperty(SkillModel.SkillState.PROPERTY_NAME, skillState);
        }
    }
}
