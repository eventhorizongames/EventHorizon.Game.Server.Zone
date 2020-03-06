/// <summary>
/// This script will set the property on the given state to the given value.
/// 
/// Arg 1: Entity Global Id
/// Arg 2: State to update
/// Arg 3: Property to update
/// Arg 4: Value to set property to
/// Arg 5: Value primitive type
/// 
/// Example Runs
/// set-property-on-state global-id OwnerState DefaultBehaviorTreeId some_behavior_tree string
/// 
/// Data: IDictionary<string, object>
/// - Command: <see cref="EventHorizon.Zone.System.Admin.Plugin.Command.Model.IAdminCommand" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;

var command = Data.Get<IAdminCommand>("Command");

var globalId = command.Parts[0];
var stateToUpdate = command.Parts[1];
var propertyToUpdate = command.Parts[2];
var propertyValue = command.Parts[3];
var propertyType = command.Parts[4];

// TODO: Validate Args/Parts

var entity = await GetEntity(globalId);
if (!entity?.IsFound() ?? true)
{
    return new AdminCommandScriptResponse(
        false,
        "entity_not_found"
    );
}

var entityState = entity.GetProperty<dynamic>(
    stateToUpdate
);

// TODO: Validate entityState

try
{
    entityState[propertyToUpdate] = CastPropertyValue(
        propertyValue,
        propertyType
    );
}
catch (InvalidCastException)
{
    return new AdminCommandScriptResponse(
        false,
        "invalid_property_type"
    );
}
catch (Exception)
{
    return new AdminCommandScriptResponse(
        false,
        "invalid_property_cast"
    );
}

// Set State back on Entity
entity.SetProperty(
    stateToUpdate,
    (object)entityState
);

await Services.Mediator.Send(
    new UpdateEntityCommand(
        EntityAction.PROPERTY_CHANGED,
        entity
    )
);

return new AdminCommandScriptResponse(
    true, // Success
    "entity_set_property_success" // Message
);

public async Task<IObjectEntity> GetEntity(
    string globalId
)
{
    var entityList = await Services.Mediator.Send(
        new QueryForEntities
        {
            Query = entity => entity.GlobalId == globalId,
        }
    );
    return entityList.FirstOrDefault();
}

public object CastPropertyValue(
    string propertyValue,
    string propertyType
)
{
    switch (propertyType.ToLower())
    {
        case "long":
            return long.Parse(propertyValue);
        case "int":
            return int.Parse(propertyValue);
        case "string":
            return propertyValue;
        case "bool":
            return bool.Parse(propertyValue);
        default:
            throw new InvalidCastException();
    }
}