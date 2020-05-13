/// <summary>
/// This script will register a new Agent.
/// 
/// Arg 1: Count to Create
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

// Find an Agent to clone
var agentList = await Services.Mediator.Send(
    new GetAgentListEvent()
);
var count = 1;
var command = Data.Get<IAdminCommand>("Command");
var countToCreateString = command.Parts.FirstOrDefault();
if (!int.TryParse(
    countToCreateString,
    out count
))
{
    count = 1;
}

for (int i = 0; i < count; i++)
{
    await Services.Mediator.Send(
        new RegisterAgentEvent(
            AgentFromDetailsToEntity.MapToNew(
                AgentFromEntityToDetails.Map(
                    agentList.LastOrDefault()
                ),
                Guid.NewGuid().ToString()
            )
        )
    );
}

return new AdminCommandScriptResponse(
    true, // Success
    "new_agent_registered" // Message
);