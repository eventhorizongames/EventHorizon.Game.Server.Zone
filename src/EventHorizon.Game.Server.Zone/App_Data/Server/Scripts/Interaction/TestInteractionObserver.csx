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
///

using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using EntityRegister = EventHorizon.Zone.Core.Events.Entity.Register;
using Logging = Microsoft.Extensions.Logging;
using ObserverModel = EventHorizon.Observer.Model;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class TestInteractionObserverEvent
    : ServerScriptsModel.ObserverableMessageBase<
          TestInteractionObserverEvent,
          TestInteractionObserverEventObserver
      >
{
    public string EventMessage { get; set; }
}

public interface TestInteractionObserverEventObserver
    : ObserverModel.ArgumentObserver<TestInteractionObserverEvent> { }

public class __SCRIPT__
    : ServerScriptsModel.ServerScript,
      TestInteractionObserverEventObserver,
      EntityRegister.EntityUnRegisterEventObserver
{
    public string Id => "__SCRIPT__";
    public Collections.IEnumerable<string> Tags => new Collections.List<string> { "testing-tag" };

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

        _logger.LogInformation($"I am here: Player: ");

        return new StandardServerScriptResponse(true, "observer_setup");
    }

    public Task Handle(EntityRegister.EntityUnRegisteredEvent args)
    {
        _logger.LogDebug("Testing");
        InMemoryGameState.Instance.RemovePlayer(_services, args.EntityId);
        return Task.CompletedTask;
    }

    public Task Handle(TestInteractionObserverEvent args)
    {
        var message = args.EventMessage;

        var score = 0;
        if (!_services.DataStore.TryGetValue<int>("Score", out score))
        {
            _logger.LogInformation("Did not find value");
        }
        score += 1;

        var scriptState = default(CustomScriptState);
        if (!_services.DataStore.TryGetValue<CustomScriptState>("ScoreState", out scriptState))
        {
            scriptState = new CustomScriptState();
        }

        scriptState.Score += 1;

        _logger.LogInformation(
            $"Message in Observer Handler: {message} | Score: {score} | ScriptState.Score {scriptState.Score}"
        );

        _services.DataStore.AddOrUpdate("Score", score);
        _services.DataStore.AddOrUpdate("ScoreState", scriptState);

        return Task.CompletedTask;
    }
}

public class CustomScriptState
{
    public int Score { get; set; }
}
