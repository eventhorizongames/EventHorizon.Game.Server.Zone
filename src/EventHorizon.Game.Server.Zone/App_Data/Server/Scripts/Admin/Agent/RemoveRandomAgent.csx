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
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;

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
        new UnRegisterAgent(
            agentList.LastOrDefault().AgentId
        )
    );
}

return new AdminCommandScriptResponse(
    true, // Success
    "new_agent_registered" // Message
);