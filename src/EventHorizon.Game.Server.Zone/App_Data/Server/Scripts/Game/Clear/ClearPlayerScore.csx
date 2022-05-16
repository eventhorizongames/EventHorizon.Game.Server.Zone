using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using Logging = Microsoft.Extensions.Logging;

using ScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class __SCRIPT__ : ScriptsModel.ObserverableMessageBase<__SCRIPT__, __SCRIPT__Observer>
{
    public long EntityId { get; }

    public __SCRIPT__(long entityId)
    {
        EntityId = entityId;
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

        InMemoryGameState.Instance.RemovePlayer(_services, args.EntityId);

        // Publish Game State Changed Action to All Clients
        await _services.Mediator.Publish(
            ClientActionGameStateChangedToAllEvent.Create(
                new GameStateChangedData
                {
                    GameState = InMemoryGameState.Instance.CurrentGameState,
                }
            )
        );
        // END - Insert Code Here
    }
}
