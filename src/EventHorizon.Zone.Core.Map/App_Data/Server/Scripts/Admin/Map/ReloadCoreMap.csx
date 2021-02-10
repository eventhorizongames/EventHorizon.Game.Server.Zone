/// <summary>
/// This will reload the Core Map.
/// 
/// Data: IDictionary<string, object>
/// - Command: <see cref="EventHorizon.Zone.System.Admin.Plugin.Command.Model.IAdminCommand" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using EventHorizon.Zone.Core.Map.Load;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;

using System.Collections.Generic;
using EventHorizon.Zone.System.Server.Scripts.Model;
using System.Threading.Tasks;
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

        await services.Mediator.Send(
            new LoadCoreMap()
        );

        return new AdminCommandScriptResponse(
            true, // Success
            "core_map_reloaded" // Message
        );
    }
}