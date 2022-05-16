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

using System.Threading.Tasks;
using CoreModels = EventHorizon.Zone.Core.Model.Entity;
using InteractionModels = EventHorizon.Zone.System.Interaction.Model;
using PlayerModels = EventHorizon.Zone.Core.Model.Player;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class __SCRIPT__ : ServerScriptsModel.ServerScript
{
    public string Id => "__SCRIPT__";
    public System.Collections.Generic.IEnumerable<string> Tags =>
        new System.Collections.Generic.List<string> { "testing-tag" };

    public async Task<ServerScriptsModel.ServerScriptResponse> Run(
        ServerScriptsModel.ServerScriptServices services,
        ServerScriptsModel.ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        var player = data.Get<PlayerModels.PlayerEntity>("Player");
        var interaction = data.Get<InteractionModels.InteractionItem>("Interaction");
        var target = data.Get<CoreModels.IObjectEntity>("Target");

        InMemoryGameState.Instance.IncrementPlayer(services, player.Id);

        logger.LogInformation($"I am here: Player: {player.Id}  |  Target: {target.Id}");

        services.ObserverBroker.Trigger(
            new TestInteractionObserverEvent
            {
                EventMessage =
                    $"[Triggered] :: I am here: Player: {player.Id}  |  Target: {target.Id}"
            }
        );

        return new ServerScriptsModel.StandardServerScriptResponse(true, "tested_interation");
    }
}
