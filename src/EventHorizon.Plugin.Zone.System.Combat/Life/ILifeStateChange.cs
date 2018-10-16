using System;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;

namespace EventHorizon.Plugin.Zone.System.Combat.Life
{
    public interface ILifeStateChange
    {
        LifeStateChangeResponse Change(IObjectEntity entity, LifeProperty property, int points);
    }
    public struct LifeStateChangeResponse
    {
        public bool Success { get; }
        public IObjectEntity ChangedEntity { get; }
        public LifeStateChangeResponse(
            bool success,
            IObjectEntity changedEntity
        )
        {
            this.Success = success;
            ChangedEntity = changedEntity;
        }
    }
}