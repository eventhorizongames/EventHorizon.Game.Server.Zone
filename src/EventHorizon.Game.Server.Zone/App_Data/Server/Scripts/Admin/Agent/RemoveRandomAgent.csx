/// <summary>
/// This script will remove 1+ random Agent.
/// 
/// Arg 1: Count To Remove (default: 1)
/// 
/// Data: IDictionary<string, object>
/// - Command: <see cref="EventHorizon.Zone.System.Admin.Plugin.Command.Model.IAdminCommand" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
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

        // Find an Agent to clone
        var agentList = await services.Mediator.Send(
            new GetAgentListEvent()
        );
        var count = 1;
        var command = data.Get<IAdminCommand>("Command");
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
            await services.Mediator.Send(
                new UnRegisterAgent(
                    agentList.LastOrDefault().AgentId
                )
            );
        }

        return new AdminCommandScriptResponse(
            true, // Success
            "random_agent_unregistered" // Message
        );
    }
}
