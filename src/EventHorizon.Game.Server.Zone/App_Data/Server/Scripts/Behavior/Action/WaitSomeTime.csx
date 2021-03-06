/// <summary>
/// Name: Behavior_Action_WaitSomeTime.csx
/// 
/// This script will wait a random amount of time before giving a Success.
/// 
/// Will Check for Running, 
/// 
/// If Running
///     Check state of timer
///     If Finished
///         Return Success
///     Else 
///         Return Running
/// Else
///     Set new future TimeSpan
///     Return Running
///  
/// Data: 
/// - Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System.Numerics;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
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

        var actor = data.Get<IObjectEntity>("Actor");

        Nullable<DateTime> nextRunDate = actor.GetProperty<DateTime?>("NextRunTime");
        if (!nextRunDate.HasValue)
        {
            // Not Found, so lets make once so we wait some time
            nextRunDate = DateTime.Now.AddSeconds(
                services.Random.Next(
                    1, // TODO: Pass this in from some setting
                    20 // TODO: Pass this in from some setting
                )
            );
            // Add MoveToNode to Actor State
            actor.SetProperty(
                "NextRunTime",
                nextRunDate
            );
            return new BehaviorScriptResponse(
                BehaviorNodeStatus.RUNNING
            );
        }

        var canRunNext = DateTime.Now.CompareTo(
            nextRunDate.Value
        ) >= 0;

        if (canRunNext)
        {
            actor.SetProperty<DateTime?, IObjectEntity>(
                "NextRunTime",
                null
            );
            return new BehaviorScriptResponse(
                BehaviorNodeStatus.SUCCESS
            );
        }

        return new BehaviorScriptResponse(
            BehaviorNodeStatus.RUNNING
        );
    }
}
