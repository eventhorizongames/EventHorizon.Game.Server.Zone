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

using EventHorizon.Game.Server.Zone.Model.Admin;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;

using EventHorizon.Game.Server.Zone.Server.Load;

var command = Data["Command"] as IAdminCommand;
await Services.Mediator.Send(
    new LoadServerScripts()
);

return new AdminCommandScriptResponse(
    true, // Success
    "server_scripts_reloaded" // Message
);