/// <summary>
/// Name: Interaction_DialogInteraction.csx
/// 
/// This script is used to initiate the Dialog plugin.
/// 
/// Data: {
///     Interaction: InteractionItem;
///     Player: PlayerEntity;
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

using EventHorizon.Zone.System.Interaction.Model.Client;
using EventHorizon.Zone.System.Interaction.Events.Client;
using EventHorizon.Zone.System.Interaction.Model;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.Core.Model.Entity;

var player = Data.Get<PlayerEntity>("Player");
var interaction = Data.Get<InteractionItem>("Interaction");
var target = Data.Get<IObjectEntity>("Target");
await Services.Mediator.Publish(
    SendSingleInteractionClientActionEvent.Create(
        player.ConnectionId,
        new InteractionClientActionData(
            "Systems.Dialog.OPEN_DIALOG_TREE_COMMAND",
            new
            {
                DialogTreeId = interaction.Data["dialogTreeId"] as string,
                PlayerId = player.Id,
                NpcId = target.Id
            }
        )
    )
);


System.Console.WriteLine("I am here");