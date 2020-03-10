/// <summary>
/// Name: Behavior_Debugging_SlowDown.csx
/// 
/// This script will stay in RUNNING for 2 seconds.
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
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.Core.Model.Entity;

System.Console.WriteLine("Slow Down Running...");
var actor = Data.Get<IObjectEntity>("Actor");

Nullable<DateTime> nextRunDate = actor.GetProperty<DateTime?>("SlowDown:NextRunTime");
if (!nextRunDate.HasValue)
{
    // Not Found, so lets make once so we wait some time
    nextRunDate = DateTime.Now.AddSeconds(2);
    // Add MoveToNode to Actor State
    actor.SetProperty<DateTime?>(
        "SlowDown:NextRunTime",
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
    actor.SetProperty<DateTime?>(
        "SlowDown:NextRunTime",
        null
    );
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.SUCCESS
    );
}

return new BehaviorScriptResponse(
    BehaviorNodeStatus.RUNNING
);