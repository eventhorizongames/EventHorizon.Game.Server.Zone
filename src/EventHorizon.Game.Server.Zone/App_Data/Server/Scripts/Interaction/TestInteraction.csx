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
using EventHorizon.Zone.System.Interaction.Model;

var player = Data.Get<PlayerEntity>("Player");
var interaction = Data.Get<InteractionItem>("Interaction");
var target = Data.Get<IObjectEntity>("Target");

System.Console.WriteLine(
    $"I am here: Player: {player.Id}  |  Target: {target.Id}"
);