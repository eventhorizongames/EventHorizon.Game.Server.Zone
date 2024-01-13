namespace EventHorizon.Zone.System.Combat.Life.Change;

using EventHorizon.Zone.Core.Model.Entity;

public interface IChangeLifeProperty
{
    LifeStateChangeResponse Change(IObjectEntity entity, long points);
}
