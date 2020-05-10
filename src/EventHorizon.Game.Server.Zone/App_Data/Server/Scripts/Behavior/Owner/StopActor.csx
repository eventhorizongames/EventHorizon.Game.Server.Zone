/// <summary>
/// Name: Behavior_Owner_StopActor.csx
/// 
/// Will remove the path of the Actor from PathState.
/// 
/// Data:
///     Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices"></see>
/// </summary>

using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.Path;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

// TODO: Promote this logic into a Agent System Event
var entity = Data.Get<IObjectEntity>("Actor");
var pathState = entity.GetProperty<PathState>(PathState.PROPERTY_NAME);
pathState = pathState.SetPath(null);
entity.SetProperty(
    PathState.PROPERTY_NAME,
    pathState
);
await Services.Mediator.Send(
    new UpdateEntityCommand(
        AgentAction.PATH,
        entity
    )
);
await Services.Mediator.Publish(
    new AgentFinishedMoveEvent
    {
        EntityId = entity.Id,
    }
);
return new BehaviorScriptResponse(
    BehaviorNodeStatus.SUCCESS
);