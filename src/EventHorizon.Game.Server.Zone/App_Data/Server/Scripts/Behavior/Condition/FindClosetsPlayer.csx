/// <summary>
/// Name: Behavior_Action_WaitSomeTime.csx
/// 
/// This script will wait a random amount of time before giving a Success.
/// 
/// Will Check for Running, 
/// 
/// If Running
///     Check state of timer
/// Else
///     Set new future TimeSpan
/// 
/// Data: 
/// - Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System.Numerics;
using System.Linq;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Events.Entity.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

var actor = Data.Get<IObjectEntity>("Actor");
var distance = 25; // TODO: Move this to Actor State
var palyerEntityId = await GetIdOfPlayerInArea(
    distance
);

if (palyerEntityId > -1)
{
    actor.SetProperty(
        "Behavior:RunFromPlayer.csx:RunFromPlayerId",
        palyerEntityId
    );
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.SUCCESS
    );
}

return new BehaviorScriptResponse(
    BehaviorNodeStatus.FAILED
);

/// <summary>
/// Searchs for any player within the distance passed in.
/// </summary>
/// <param name="distance">The Units to search for player in.</param>
/// <returns>The Id of the player found, otherwise -1.</returns>
private async Task<long> GetIdOfPlayerInArea(
    int distance
)
{
    // Find the nearest player, within a certain distance/radius
    var entityList = await Services.Mediator.Send(
            new SearchInAreaWithTagEvent
            {
                SearchPositionCenter = actor.Transform.Position,
                SearchRadius = distance,
                TagList = new List<string> { "player" }
            }
        );

    if (entityList.Count() >= 1)
    {
        return entityList.First();
    }
    return -1;
}