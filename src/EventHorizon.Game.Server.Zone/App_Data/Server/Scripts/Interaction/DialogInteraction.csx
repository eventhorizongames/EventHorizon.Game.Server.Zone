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


using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        var player = data.Get<PlayerEntity>("Player");
        var interaction = data.Get<InteractionItem>("Interaction");
        var target = data.Get<IObjectEntity>("Target");
        await services.Mediator.Publish(
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
        
        return new StandardServerScriptResponse(
            true,
            "published_dialog_interation"
        );
    }
}
