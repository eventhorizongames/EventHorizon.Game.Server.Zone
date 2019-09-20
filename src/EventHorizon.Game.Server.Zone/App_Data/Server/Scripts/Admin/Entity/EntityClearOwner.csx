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
using EventHorizon.Zone.Core.Model.Admin;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;
using EventHorizon.Game.Server.Zone.Entity.Find;

var command = Data.Command;
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
            Query = (a => a.Type != EntityType.PLAYER && a.GlobalId == globalId)
        }
    );
if (entityList.Count() != 1)
{
    return new AdminCommandScriptResponse(
        false, // Success
        "invalid_entity_id" // Message
    );
}

entityList.First().GetProperty<dynamic>("ownerState")["ownerId"] = "";
entityList.First().GetProperty<dynamic>("ownerState")["canBeCaptured"] = true;

return new AdminCommandScriptResponse(
    true, // Success
    "entity_owner_cleared" // Message
);