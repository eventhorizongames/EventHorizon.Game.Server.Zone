/// <summary>
/// Name: Behavior_Owner_IsFarFromOwner.csx
/// 
/// This script will find the owner location and compare it to the actor/agent location.
///
/// if the owner is farther than 5 units
///  it will move to that location
/// if not
///  it will fail the check
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
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

var delta = 5; // TODO: Get this from Actor setting : Close To Owner Distance

var actor = Data.Get<IObjectEntity>("Actor");
var owner = await GetOwner(
    actor
);

// Get distance between actor to owner
var distance = Vector3.Distance(
    actor.Position.CurrentPosition,
    owner.Position.CurrentPosition
);

System.Console.WriteLine("Distance from Actor to Owner: " + distance);

// They are not far from teh owner, within delta
return new BehaviorScriptResponse(
    BehaviorNodeStatus.FAILED
);


public async Task<IObjectEntity> GetOwner(
    IObjectEntity actor
)
{
    // Get Owner
    var ownerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);
    // Get Current Owner of Actor of type Player
    var ownerList = await Services.Mediator.Send(
        new QueryForEntities
        {
            Query = (
                entity => entity.Type == EntityType.PLAYER
                && entity.GlobalId == ownerState.OwnerId
            )
        }
    );
    return ownerList.FirstOrDefault();
}