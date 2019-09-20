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

using EventHorizon.Zone.Core.Model.Admin;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;

using EventHorizon.Plugin.Zone.System.Combat.Load;

var command = Data.Command;
await Services.Mediator.Publish(new LoadCombatSystemEvent());

return new AdminCommandScriptResponse(
    true, // Success
    "combat_system_reloaded" // Message
);