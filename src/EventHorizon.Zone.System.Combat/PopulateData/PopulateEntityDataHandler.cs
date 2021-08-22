namespace EventHorizon.Zone.System.Combat.PopulateData
{
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Model;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class PopulateEntityDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(PopulateEntityDataEvent notification, CancellationToken cancellationToken)
        {
            var entity = notification.Entity;

            entity.PopulateData<LifeState>(
                LifeState.PROPERTY_NAME
            );
            entity.PopulateData<LevelState>(
                LevelState.PROPERTY_NAME
            );

            ValidateLifeState(
                entity
            );

            return Task.CompletedTask;
        }

        private void ValidateLifeState(
            IObjectEntity entity
        )
        {
            var levelState = entity.GetProperty<LevelState>(
                LevelState.PROPERTY_NAME
            );

            if (levelState.AllTimeExperience == 0)
            {
                entity.SetProperty(
                    LifeState.PROPERTY_NAME,
                    LifeState.NEW
                );
                entity.SetProperty(
                    LevelState.PROPERTY_NAME,
                    LevelState.NEW
                );
            }
        }
    }
}
