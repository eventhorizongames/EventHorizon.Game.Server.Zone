using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using FindPlayer = EventHorizon.Zone.System.Player.Events.Find;
using Logging = Microsoft.Extensions.Logging;
using ScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

// __SCRIPT__ - RunCaptureLogicForPlayerEvent
public class __SCRIPT__ : ScriptsModel.ObserverableMessageBase<__SCRIPT__, __SCRIPT__Observer>
{
    public long PlayerEntityId { get; }

    public __SCRIPT__(long playerEntityId)
    {
        PlayerEntityId = playerEntityId;
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
        var logger = _services.Logger<__SCRIPT__>();
        var player = await _services.Mediator.Send(
            new FindPlayer.FindPlayerByEntityId(args.PlayerEntityId)
        );
        var captureState = player.GetProperty<GamePlayerCaptureState>(
            GamePlayerCaptureState.PROPERTY_NAME
        );
        var escapeCaptureTime = captureState.EscapeCaptureTime;
        var captures = captureState.Captures;

        if (captures == 0)
        {
            return;
        }

        if (ShouldProcessTenSecondMessage(captureState, escapeCaptureTime))
        {
            await _services.ObserverBroker.Trigger(
                new Game_Capture_Logic_ProcessTenSecondCaptureLogic(player)
            );
        }

        if (ShouldProcessFiveSecondMessage(captureState, escapeCaptureTime))
        {
            await _services.ObserverBroker.Trigger(
                new Game_Capture_Logic_ProcessFiveSecondCaptureLogic(player)
            );
        }

        if (ShouldTriggerEscapeOfCaptures(escapeCaptureTime))
        {
            await _services.ObserverBroker.Trigger(
                new Game_Capture_Logic_RunEscapeOfCaptures(player)
            );
        }
        logger.LogDebug($"__SCRIPT__ - End Player Script - {args.PlayerEntityId}");
    }

    private bool ShouldProcessTenSecondMessage(
        GamePlayerCaptureState captureState,
        DateTime escapeCaptureTime
    )
    {
        return escapeCaptureTime.CompareTo(
                _services.DateTime.Now.AddSeconds(10) // Within 10 Seconds
            ) <= 0
            && !captureState.ShownTenSecondMessage;
    }

    private bool ShouldProcessFiveSecondMessage(
        GamePlayerCaptureState captureState,
        DateTime escapeCaptureTime
    )
    {
        return escapeCaptureTime.CompareTo(
                _services.DateTime.Now.AddSeconds(5) // Within 5 Seconds
            ) <= 0
            && !captureState.ShownFiveSecondMessage;
    }

    private bool ShouldTriggerEscapeOfCaptures(DateTime escapeCaptureTime)
    {
        return escapeCaptureTime.CompareTo(_services.DateTime.Now) <= 0;
    }
}
