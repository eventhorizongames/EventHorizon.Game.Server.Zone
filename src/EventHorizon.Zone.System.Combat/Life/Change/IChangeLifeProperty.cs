using System;

using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Combat.Life.Change
{
    public interface IChangeLifeProperty
    {
        LifeStateChangeResponse Change(IObjectEntity entity, long points);
    }
}
