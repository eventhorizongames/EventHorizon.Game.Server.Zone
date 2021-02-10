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

        System.Console.WriteLine(
            $"I am here: Player: {player.Id}  |  Target: {target.Id}"
        );

        return new StandardServerScriptResponse(
            true,
            "tested_interation"
        );
    }
}
