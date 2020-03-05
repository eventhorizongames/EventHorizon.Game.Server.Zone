/// <summary>
/// Name: Behavior_Owner_HasNoOwner.csx
/// 
/// Will check if the current Actor has a Valid Owner.
/// Returns False, when they have an Owner
/// 
/// If No Owner
///     Return SUCCESS
/// Else If Owner Not Found
///     Return SUCCESS
/// Else
///     Return FAILED
/// 
/// Actor: { 
///     Id: long; 
///     BehaviorState: IBehaviorState;
/// } 
/// Services: { 
///     Mediator: IMediator; 
///     Random: IRandomNumberGenerator; 
///     DateTime: IDateTimeService; 
///     I18n: I18nLookup; 
/// }
/// </summary>

using System.Linq;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

var actor = Data.Get<IObjectEntity>("Actor");
var ownerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);

// The Actor is not an Agent Entity, only agents can have owners.
if (!(actor is AgentEntity))
{
    // Actor is not a valid Agent Entity
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.SUCCESS
    );
}

// Check Owner is valid
if (string.IsNullOrEmpty(ownerState.OwnerId))
{
    // OwnerId not valid, so no owner
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.SUCCESS
    );
}

// Get Current Owner of Actor of type Player
var ownerList = await Services.Mediator.Send(
    new QueryForEntities
    {
        // Entity Must be a Player and have a Global Id same as the Owner's
        Query = (
            entity => entity.Type == EntityType.PLAYER
            && entity.GlobalId == ownerState.OwnerId
        )
    }
);

// Check the count of the owner list
var ownerListCount = ownerList.Count();
if (ownerListCount == 0 || ownerListCount >= 2)
{
    // No owner or to many owners, so no owner
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.SUCCESS
    );
}

// They have an owner so this script fails.
return new BehaviorScriptResponse(
    BehaviorNodeStatus.FAILED
);
