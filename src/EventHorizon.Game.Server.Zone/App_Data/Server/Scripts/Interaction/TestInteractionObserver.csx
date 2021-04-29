/// <summary>
/// Name: Interaction_TestInteraction.csx
/// 
/// This script is just a test script, writes to the log.
/// 
/// Data: {
///     Interaction: InteractionItem;    
///     Player: IObjectEntity;
///     Target: IObjectEntity;
/// }
/// Services: { 
///     Mediator: IMediator; 
///     Random: IRandomNumberGenerator; 
///     DateTime: IDateTimeService; 
///     I18n: I18nLookup; 
/// }
/// 
/// InteractionItem: {
///     ScriptId: string;
///     DistanceToPlayer: int;
///     Data: IDictionary<string, object>
/// }
/// </summary>

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class TestInteractionObserverEvent
{
    public string EventMessage { get; set; }
}

public interface TestInteractionObserverEventObserver
    : ArgumentObserver<TestInteractionObserverEvent>
{
}

public class __SCRIPT__
    : ServerScript,
    TestInteractionObserverEventObserver
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    private ServerScriptServices _services;
    private ILogger _logger;

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        _services = services;
        _logger = services.Logger<__SCRIPT__>();
        _logger.LogDebug("__SCRIPT__ - Server Script");

        _logger.LogInformation(
            $"I am here: Player: "
        );

        return new StandardServerScriptResponse(
            true,
            "observer_setup"
        );
    }

    public Task Handle(
        TestInteractionObserverEvent args
    )
    {
        var message = args.EventMessage;

        _logger.LogInformation(
            $"Message in Observer Handler: {message}"
        );

        return Task.CompletedTask;
    }
}