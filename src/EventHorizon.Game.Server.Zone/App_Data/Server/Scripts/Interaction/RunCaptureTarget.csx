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
using EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;

var player = Data.Get<PlayerEntity>("Player");
var target = Data.Get<IObjectEntity>("Target");

await Services.Mediator.Publish(
    new RunSkillWithTargetOfEntityEvent
    {
        Player = player,
        ConnectionId = player.ConnectionId,
        SkillId = "Skills_CaptureTarget.json",
        CasterId = player.Id,
        TargetId = target.Id
    }
);
