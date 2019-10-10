/// <summary>
/// Effect Id: entity-clear-owner
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

using System.Linq;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Events.Entity.Find;

var command = Data.Get<IAdminCommand>("Command");
if (command.Parts.Count != 1)
{
    return new AdminCommandScriptResponse(
        false, // Success
        "not_valid_command" // Message
    );
}
var globalId = command.Parts[0];
// Using Query find Entities
var entityList = await Services.Mediator.Send(
        new QueryForEntities
        {
            Query = (
                entity => entity.Type != EntityType.PLAYER 
                && entity.GlobalId == globalId
            )
        }
    );
if (entityList.Count() != 1)
{
    return new AdminCommandScriptResponse(
        false, // Success
        "invalid_entity_id" // Message
    );
}

var ownerState = entityList.First().GetProperty<dynamic>("ownerState");
ownerState["ownerId"] = "";
ownerState["canBeCaptured"] = true;

return new AdminCommandScriptResponse(
    true, // Success
    "entity_owner_cleared" // Message
);