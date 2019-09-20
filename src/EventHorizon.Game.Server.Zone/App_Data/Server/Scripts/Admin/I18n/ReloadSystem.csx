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
using EventHorizon.Game.I18n.Loader;

var command = Data.Command;
await Services.Mediator.Publish(
    new I18nLoadEvent()
);

return new AdminCommandScriptResponse(
    true, // Success
    "i18n_system_reloaded" // Message
);