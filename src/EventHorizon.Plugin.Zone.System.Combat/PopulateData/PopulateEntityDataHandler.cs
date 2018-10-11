using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Data;
using MediatR;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.PopulateData
{
    public class PopulateEntityDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(PopulateEntityDataEvent notification, CancellationToken cancellationToken)
        {
            var entity = notification.Entity;

            entity.PopulateData<LifeState>(LifeState.PROPERTY_NAME);

            var lifeState = entity.GetProperty<LifeState>(LifeState.PROPERTY_NAME);

            return Task.CompletedTask;
        }
    }
}