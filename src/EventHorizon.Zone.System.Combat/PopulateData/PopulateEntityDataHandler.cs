using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Data;
using MediatR;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Model;

namespace EventHorizon.Zone.System.Combat.PopulateData
{
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

            this.ValidateLifeState(
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