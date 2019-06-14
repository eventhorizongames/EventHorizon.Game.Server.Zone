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

using EventHorizon.Game.Server.Zone.Model.Admin;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;

using EventHorizon.Game.Server.Zone.Agent.Ai.LoadRoutine;

var command = Data["Command"] as IAdminCommand;
await Services.Mediator.Publish(new LoadAgentRoutineSystemEvent());

return new AdminCommandScriptResponse(
    true, // Success
    "agent_routine_system_reloaded" // Message
);