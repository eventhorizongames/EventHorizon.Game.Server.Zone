/// <summary>
/// This script will set the agent behavior.
/// 
/// Arg 1: Global Id
/// Arg 2: Behavior Tree Id
/// 
/// Data: IDictionary<string, object>
/// - Command: <see cref="EventHorizon.Zone.System.Admin.Plugin.Command.Model.IAdminCommand" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System.Linq;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

var command = Data.Get<IAdminCommand>("Command");
if (command.Parts.Count != 2)
{
    return new AdminCommandScriptResponse(
        false, // Failure
        "not_valid_command" // Message
    );
}
var globalId = command.Parts[0];
var behaviorTreeId = command.Parts[1];

if (string.IsNullOrEmpty(behaviorTreeId) || string.IsNullOrEmpty(globalId))
{
    return new AdminCommandScriptResponse(
        false, // Failure
        "set_agent_behavior_args_invalid" // Message
    );
}

var entityList = await Services.Mediator.Send(
    new QueryForEntities
    {
        Query = entity => entity.GlobalId == globalId,
    }
);
var entity = entityList.FirstOrDefault();
if (!entity?.IsFound() ?? true)
{
    return new AdminCommandScriptResponse(
        false, // Failed
        "agent_not_found" // Message
    );
}

var wasChanged = await Services.Mediator.Send(
    new ChangeActorBehaviorTreeCommand(
        entity,
        behaviorTreeId
    )
);

if (!wasChanged)
{
    return new AdminCommandScriptResponse(
        false, // Success
        "agent_behavior_change_failed" // Message
    );
}

return new AdminCommandScriptResponse(
    true, // Success
    "agent_behavior_set" // Message
);