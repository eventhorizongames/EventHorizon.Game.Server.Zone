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
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.Core.Model.Entity;

var actor = Data.Get<IObjectEntity>("Actor");

Nullable<DateTime> nextRunDate = actor.GetProperty<DateTime?>("NextRunTime");
if (!nextRunDate.HasValue)
{
    // Not Found, so lets make once so we wait some time
    nextRunDate = DateTime.Now.AddSeconds(
        Services.Random.Next(
            1, // TODO: Pass this in from some setting
            20 // TODO: Pass this in from some setting
        )
    );
    // Add MoveToNode to Actor State
    actor.SetProperty<DateTime?>(
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
    actor.SetProperty<DateTime?>(
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