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

// TODO: Do some logic. ;)
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

return new BehaviorScriptResponse(
    BehaviorNodeStatus.RUNNING
);