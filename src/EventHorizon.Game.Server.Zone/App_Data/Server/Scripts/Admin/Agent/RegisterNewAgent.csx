/// <summary>
/// This will publish the I18nLoadEvent.
/// 
/// Data: IDictionary<string, object>
/// - Command
///  - RawCommand: string;
///  - Command: string;
///  - Parts: IList<string>;
/// Services: 
/// - Mediator: IMediator;
/// - I18n: I18nLookup;
/// </summary>

using System.Linq;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;
using EventHorizon.Zone.Core.Model.Admin;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Save.Mapper;

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
        new RegisterAgentEvent
        {
            Agent = AgentFromDetailsToEntity.MapToNew(
                AgentFromEntityToDetails.Map(
                    agentList.LastOrDefault()
                ),
                Guid.NewGuid().ToString()
            )
        }
    );
}

return new AdminCommandScriptResponse(
    true, // Success
    "new_agent_registered" // Message
);