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

using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.EntityModule.Load;

var command = Data.Get<IAdminCommand>("Command");
await Services.Mediator.Publish(
    new LoadEntityModuleSystemCommand()
);

return new AdminCommandScriptResponse(
    true, // Success
    "entity_module_system_reloaded" // Message
);