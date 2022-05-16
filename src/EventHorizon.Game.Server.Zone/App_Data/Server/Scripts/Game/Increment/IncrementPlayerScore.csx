using System.Collections.Generic;
using System.Threading.Tasks;
using Logging = Microsoft.Extensions.Logging;
using ObserverModel = EventHorizon.Observer.Model;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class __SCRIPT__ : ScriptsModel.ObserverableMessageBase<__SCRIPT__, __SCRIPT__Observer>
{
    public long PlayerEntityId { get; }

    public __SCRIPT__(long playerEntityId)
    {
        PlayerEntityId = playerEntityId;
    }
}

public interface __SCRIPT__Observer : ObserverModel.ArgumentObserver<__SCRIPT__> { }

public class __SCRIPT__Handler : ServerScriptsModel.ServerScript, __SCRIPT__Observer
{
    public string Id => "__SCRIPT__";
    public Collections.IEnumerable<string> Tags => new List<string> { };

    private ServerScriptsModel.ServerScriptServices _services;
    private Logging.ILogger _logger;

    public async Task<ServerScriptsModel.ServerScriptResponse> Run(
        ServerScriptsModel.ServerScriptServices services,
        ServerScriptsModel.ServerScriptData data
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

        // Increment Player Score on Game State
        InMemoryGameState.Instance.IncrementPlayer(_services, args.PlayerEntityId);
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
