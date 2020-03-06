/// <summary>
/// This will reload the Agent Behavior System.
/// 
/// Data: IDictionary<string, object>
/// - Command: <see cref="EventHorizon.Zone.System.Admin.Plugin.Command.Model.IAdminCommand" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;

await Services.Mediator.Send(
    new LoadAgentBehaviorSystem()
);

return new AdminCommandScriptResponse(
    true, // Success
    "agent_behavior_system_reloaded" // Message
);