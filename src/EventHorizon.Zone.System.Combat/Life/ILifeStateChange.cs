namespace EventHorizon.Zone.System.Combat.Life
{
    using EventHorizon.Zone.Core.Model.Entity;

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
            Success = success;
            ChangedEntity = changedEntity;
        }
    }
}
