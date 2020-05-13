/// <summary>
/// This will publish the ReloadClientScriptsSystem event.
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
using EventHorizon.Zone.System.Client.Scripts.Reload;

var command = Data.Get<IAdminCommand>("Command");

await Services.Mediator.Send(
    new ReloadClientScriptsSystem()
);

return new AdminCommandScriptResponse(
    true, // Success
    "client_scripts_system_reloaded" // Message
);