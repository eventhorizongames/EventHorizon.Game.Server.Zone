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
/// Actor: { 
///     Id: long; 
///     BehaviorState: IBehaviorState;
/// } 
/// Services: { 
///     Mediator: IMediator; 
///     Random: IRandomNumberGenerator; 
///     DateTime: IDateTimeService; 
///     I18n: I18nLookup; 
/// }
/// </summary>

using System.Numerics;
using EventHorizon.Game.Server.Zone.Agent.Ai.Model;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;


Nullable<DateTime> nextRunDate = Actor.GetProperty<DateTime?>("NextRunTime");
if (!nextRunDate.HasValue)
{
    // Not Found, so lets make once so we wait some time
    nextRunDate = DateTime.Now.AddSeconds(
        Services.Random.Next(
            1, 20
        )
    );
    // Add MoveToNode to Actor State
    Actor.SetProperty<DateTime?>(
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
    Actor.SetProperty<DateTime?>(
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