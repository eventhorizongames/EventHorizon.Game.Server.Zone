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

using EventHorizon.Zone.System.Client.Scripts.Load;

var command = Data.Get<IAdminCommand>("Command");
await Services.Mediator.Publish(
    new LoadClientScriptsSystemCommand()
);

return new AdminCommandScriptResponse(
    true, // Success
    "client_scripts_system_reloaded" // Message
);