using ClientActionApi = EventHorizon.Game.Client.Engine.Systems.ClientAction.Api;
using ClientActionAttributes = EventHorizon.Game.Client.Engine.Systems.ClientAction.Attributes;
using ObserverModel = EventHorizon.Observer.Model;

// Game_ClientActions_ClientActionShowTenSecondCaptureMessageEvent
[ClientActionAttributes.ClientAction("Server.SHOW_TEN_SECOND_CAPTURE_MESSAGE")]
public struct __SCRIPT__ : ClientActionApi.IClientAction
{
    public __SCRIPT__(ClientActionApi.IClientActionDataResolver _) { }
}

public interface __SCRIPT__Observer : ObserverModel.ArgumentObserver<__SCRIPT__> { }
