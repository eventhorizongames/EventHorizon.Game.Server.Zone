using System.Threading.Tasks;
using AgentGet = EventHorizon.Zone.System.Agent.Events.Get;
using Collections = System.Collections.Generic;
using CompanionModel = EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
using EntityModel = EventHorizon.Zone.Core.Model.Entity;
using EntityUpdate = EventHorizon.Zone.Core.Events.Entity.Update;
using Logging = Microsoft.Extensions.Logging;
using PlayerModel = EventHorizon.Zone.Core.Model.Player;
using ScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;
using SkillEventsRunner = EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;

public class __SCRIPT__ : ScriptsModel.ObserverableMessageBase<__SCRIPT__, __SCRIPT__Observer>
{
    public PlayerModel.PlayerEntity PlayerEntity { get; }

    public __SCRIPT__(PlayerModel.PlayerEntity playerEntity)
    {
        PlayerEntity = playerEntity;
    }
}

public interface __SCRIPT__Observer : ObserverModel.ArgumentObserver<__SCRIPT__> { }

public class __SCRIPT__Handler : ScriptsModel.ServerScript, __SCRIPT__Observer
{
    public string Id => "__SCRIPT__";
    public Collections.IEnumerable<string> Tags => new Collections.List<string> { };

    private ScriptsModel.ServerScriptServices _services;
    private Logging.ILogger _logger;

    public async Task<ServerScriptResponse> Run(
        ScriptsModel.ServerScriptServices services,
        ScriptsModel.ServerScriptData data
    )
    {
        _services = services;
        _logger = services.Logger<__SCRIPT__>();
        _logger.LogDebug("__SCRIPT__ - Server Script");

        return new ScriptsModel.StandardServerScriptResponse(true, "observer_setup");
    }

    public async Task Handle(__SCRIPT__ args)
    {
        // START - Insert Code Here
        _logger.LogDebug("__SCRIPT__ - Server Script Triggered");

        var playerEntity = args.PlayerEntity;
        var captureState = playerEntity.GetProperty<GamePlayerCaptureState>(
            GamePlayerCaptureState.PROPERTY_NAME
        );
        foreach (var globalId in captureState.CompanionsCaught)
        {
            var companionEntity = await _services.Mediator.Send(
                new AgentGet.FindAgentByIdEvent(globalId)
            );
            var ownerState = companionEntity.GetProperty<CompanionModel.OwnerState>(
                CompanionModel.OwnerState.PROPERTY_NAME
            );
            ownerState.OwnerId = string.Empty;
            companionEntity = companionEntity.SetProperty(
                CompanionModel.OwnerState.PROPERTY_NAME,
                ownerState
            );
            await _services.Mediator.Send(
                new EntityUpdate.UpdateEntityCommand(
                    EntityModel.EntityAction.PROPERTY_CHANGED,
                    companionEntity
                )
            );
        }
        await _services.Mediator.Publish(
            new SkillEventsRunner.RunSkillWithTargetOfEntityEvent
            {
                ConnectionId = playerEntity.ConnectionId,
                SkillId = SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID,
                CasterId = playerEntity.Id,
                TargetId = playerEntity.Id,
                TargetPosition = playerEntity.Transform.Position,
                Data = new Dictionary<string, object>
                {
                    { "game:MessageKey", "game:CapturesEscaped" }
                }
            }
        );

        await _services.ObserverBroker.Trigger(new Game_Clear_ClearPlayerScore(playerEntity.Id));

        playerEntity = playerEntity.SetProperty(
            GamePlayerCaptureState.PROPERTY_NAME,
            GamePlayerCaptureState.New()
        );

        await _services.Mediator.Send(
            new EntityUpdate.UpdateEntityCommand(EntityAction.PROPERTY_CHANGED, playerEntity)
        );

        // END - Insert Code Here
    }
}
