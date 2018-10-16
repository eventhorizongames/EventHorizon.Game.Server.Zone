using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Data;
using MediatR;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using System;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;

namespace EventHorizon.Plugin.Zone.System.Combat.PopulateData
{
    public class PopulateEntityDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(PopulateEntityDataEvent notification, CancellationToken cancellationToken)
        {
            var entity = notification.Entity;

            entity.PopulateData<LifeState>(LifeState.PROPERTY_NAME);
            entity.PopulateData<LevelState>(LevelState.PROPERTY_NAME);

            this.ValidateLifeState(entity);


            return Task.CompletedTask;
        }

        private void ValidateLifeState(IObjectEntity entity)
        {
            var lifeState = entity.GetProperty<LifeState>(LifeState.PROPERTY_NAME);
            var levelState = entity.GetProperty<LevelState>(LevelState.PROPERTY_NAME);

            if (levelState.AllTimeExperience == 0)
            {
                entity.SetProperty(LifeState.PROPERTY_NAME, LifeState.NEW);
                entity.SetProperty(LevelState.PROPERTY_NAME, LevelState.NEW);
            }
        }
    }
}