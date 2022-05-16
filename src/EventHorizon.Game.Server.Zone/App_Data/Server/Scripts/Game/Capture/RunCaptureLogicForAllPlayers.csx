using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using EntityModel = EventHorizon.Zone.Core.Model.Entity;
using FindEntity = EventHorizon.Zone.Core.Events.Entity.Find;
using ScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class RunCaptureLogicForAllPlayersEvent
    : ScriptsModel.ObserverableMessageBase<
          RunCaptureLogicForAllPlayersEvent,
          RunCaptureLogicForAllPlayersEventObserver
      > { }

public interface RunCaptureLogicForAllPlayersEventObserver
    : ObserverModel.ArgumentObserver<RunCaptureLogicForAllPlayersEvent> { }

public class __SCRIPT__ : ScriptsModel.ServerScript, RunCaptureLogicForAllPlayersEventObserver
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

        return new StandardServerScriptResponse(true, "observer_setup");
    }

    public async Task Handle(RunCaptureLogicForAllPlayersEvent args)
    {
        var listOfPlayers = await _services.Mediator.Send(
            new FindEntity.QueryForEntities()
            {
                Query = entity => entity.Type == EntityModel.EntityType.PLAYER,
            }
        );

        foreach (var player in listOfPlayers)
        {
            await _services.ObserverBroker.Trigger(
                new Game_Capture_RunCaptureLogicForPlayer(player.Id)
            );
        }
    }
}
