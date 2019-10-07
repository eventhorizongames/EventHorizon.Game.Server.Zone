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

using EventHorizon.Zone.System.ClientEntities.Load;

var command = Data.Get<IAdminCommand>("Command");
await Services.Mediator.Send(
    new LoadSystemClientEntitiesCommand()
);

return new AdminCommandScriptResponse(
    true, // Success
    "client_entities_system_reloaded" // Message
);