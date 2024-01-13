namespace EventHorizon.Zone.System.Combat.Life.Change.Property;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Model;

public class ChangeActionPoints
    : IChangeLifeProperty
{
    public LifeStateChangeResponse Change(IObjectEntity entity, long points)
    {
        var entityLifeState = entity.GetProperty<LifeState>(LifeState.PROPERTY_NAME);

        var newActionPointValue = entityLifeState.ActionPoints + points;
        if (newActionPointValue < 0)
        {
            return new LifeStateChangeResponse(false, entity);
        }

        entityLifeState.ActionPoints = newActionPointValue;

        if (entityLifeState.ActionPoints == 0)
        {
            entityLifeState.ActionPoints = 0;
        }

        entity.SetProperty(LifeState.PROPERTY_NAME, entityLifeState);

        return new LifeStateChangeResponse(true, entity);
    }
}
