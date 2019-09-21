using System;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Model.Life;

namespace EventHorizon.Zone.System.Combat.Life
{
    public interface ILifeStateChange
    {
        LifeStateChangeResponse Change(IObjectEntity entity, string property, long points);
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