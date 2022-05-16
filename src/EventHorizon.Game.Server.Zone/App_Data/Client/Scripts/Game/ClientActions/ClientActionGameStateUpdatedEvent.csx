using ClientActionApi = EventHorizon.Game.Client.Engine.Systems.ClientAction.Api;
using ClientActionAttributes = EventHorizon.Game.Client.Engine.Systems.ClientAction.Attributes;
using ObserverModel = EventHorizon.Observer.Model;

// Game_ClientActions_ClientActionGameStateUpdatedEvent
[ClientActionAttributes.ClientAction("CLIENT_ACTION_GAME_STATE_UPDATED")]
public struct __SCRIPT__ : ClientActionApi.IClientAction
{
    public GameState GameState { get; }

    public __SCRIPT__(ClientActionApi.IClientActionDataResolver resolver)
    {
        GameState = resolver.Resolve<GameState>("gameState");
    }
}

public interface __SCRIPT__Observer : ObserverModel.ArgumentObserver<__SCRIPT__> { }
