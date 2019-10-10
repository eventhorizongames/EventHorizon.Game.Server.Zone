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

using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;

var command = Data.Get<IAdminCommand>("Command");
await Services.Mediator.Send(
    new LoadServerScriptsCommand()
);

return new AdminCommandScriptResponse(
    true, // Success
    "server_scripts_reloaded" // Message
);