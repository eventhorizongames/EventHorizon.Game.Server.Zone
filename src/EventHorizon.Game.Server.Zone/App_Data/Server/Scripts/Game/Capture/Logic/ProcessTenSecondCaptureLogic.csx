using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using EntityUpdate = EventHorizon.Zone.Core.Events.Entity.Update;
using Logging = Microsoft.Extensions.Logging;
using PlayerModel = EventHorizon.Zone.Core.Model.Player;
using ScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

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

        await _services.Mediator.Publish(
            ClientActionShowTenSecondCaptureMessageToSingleEvent.Create(
                playerEntity.ConnectionId,
                new ClientActionShowTenSecondCaptureMessageData()
            )
        );

        var captureState = playerEntity.GetProperty<GamePlayerCaptureState>(
            GamePlayerCaptureState.PROPERTY_NAME
        );
        captureState.ShownTenSecondMessage = true;
        playerEntity = playerEntity.SetProperty(GamePlayerCaptureState.PROPERTY_NAME, captureState);

        await _services.Mediator.Send(
            new EntityUpdate.UpdateEntityCommand(EntityAction.PROPERTY_CHANGED, playerEntity)
        );
        // END - Insert Code Here
    }
}
