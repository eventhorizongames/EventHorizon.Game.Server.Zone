using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Zone.System.ModelState.PopulateData
{
    public class PopulateEntityModelDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(PopulateEntityDataEvent notification, CancellationToken cancellationToken)
        {
            var entity = notification.Entity;

            entity.PopulateData<EntityModelState>(EntityModelState.PROPERTY_NAME);

            this.ValidateModelState(entity);

            return Task.CompletedTask;
        }

        private void ValidateModelState(IObjectEntity entity)
        {
            var modelState = entity.GetProperty<EntityModelState>(EntityModelState.PROPERTY_NAME);

            if (!modelState.IsValid())
            {
                entity.SetProperty(EntityModelState.PROPERTY_NAME, EntityModelState.DEFAULT);
            }
        }
    }
}