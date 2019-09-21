using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Plugin.Zone.System.Model.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Model.PopulateData
{
    public class PopulateEntityModelDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(PopulateEntityDataEvent notification, CancellationToken cancellationToken)
        {
            var entity = notification.Entity;

            entity.PopulateData<ModelState>(ModelState.PROPERTY_NAME);

            this.ValidateModelState(entity);

            return Task.CompletedTask;
        }

        private void ValidateModelState(IObjectEntity entity)
        {
            var modelState = entity.GetProperty<ModelState>(ModelState.PROPERTY_NAME);

            if (!modelState.IsValid())
            {
                entity.SetProperty(ModelState.PROPERTY_NAME, ModelState.DEFAULT);
            }
        }
    }
}