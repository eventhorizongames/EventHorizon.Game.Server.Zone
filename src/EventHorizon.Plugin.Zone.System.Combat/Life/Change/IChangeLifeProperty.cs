using System;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Plugin.Zone.System.Combat.Life.Change
{
    public interface IChangeLifeProperty
    {
        LifeStateChangeResponse Change(IObjectEntity entity, long points);
    }
}