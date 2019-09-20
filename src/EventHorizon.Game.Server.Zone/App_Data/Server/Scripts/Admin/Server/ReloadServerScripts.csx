/// <summary>
/// This will publish the LoadServerScripts.
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
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Game.Server.Zone.Server.Load;

var command = Data.Command;
await Services.Mediator.Send(
    new LoadServerScriptsCommand()
);

return new AdminCommandScriptResponse(
    true, // Success
    "server_scripts_reloaded" // Message
);