/// <summary>
/// This will reload the Core Map.
/// 
/// Data: IDictionary<string, object>
/// - Command: <see cref="EventHorizon.Zone.System.Admin.Plugin.Command.Model.IAdminCommand" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using EventHorizon.Zone.Core.Map.Load;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;

await Services.Mediator.Send(
    new LoadCoreMap()
);

return new AdminCommandScriptResponse(
    true, // Success
    "core_map_reloaded" // Message
);