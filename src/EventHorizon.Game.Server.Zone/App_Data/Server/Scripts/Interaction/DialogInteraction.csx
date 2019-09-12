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

using EventHorizon.Zone.Plugin.Interaction.Model.Client;
using EventHorizon.Zone.Plugin.Interaction.Events.Client;

Services.Mediator.Publish(
    new SendSingleInteractionClientActionEvent(
        Data.Player.ConnectionId,
        new InteractionClientActionData(
            "Systems.Dialog.OPEN_DIALOG_TREE_COMMAND",
            new {
                DialogTreeId = Data.Interaction.Data["dialogTreeId"] as string,
                PlayerId = Data.Player.Id,
                NpcId = Data.Target.Id
            }
        )
    )
);


System.Console.WriteLine("I am here");