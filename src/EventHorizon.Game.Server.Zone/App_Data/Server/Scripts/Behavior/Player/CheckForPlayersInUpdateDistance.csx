/// <summary>
/// Name: Behavior_Player_CheckForPlayersInUpdateDistance.csx
/// 
/// This script can be used to validate that the current Actor is in 
///  distance of any players based on an Update Distance value.
/// 
/// The main purpose of this script is to return success/fail so the BT procssing
///  should continue the Actors processing.
/// 
/// Data:
///     Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices"></see>
/// </summary>

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;


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
        
        // TODO: Do some logic. ;)

        return new BehaviorScriptResponse(
            BehaviorNodeStatus.RUNNING
        );
    }
}
