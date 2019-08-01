using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Data;
using MediatR;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.Plugin.Interaction.Model;

namespace EventHorizon.Plugin.Zone.Interaction.PopulateData
{
    public class PopulateEntityDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(
            PopulateEntityDataEvent notification, 
            CancellationToken cancellationToken
        )
        {
            var entity = notification.Entity;

            entity.PopulateData<InteractionState>(
                InteractionState.PROPERTY_NAME, 
                InteractionState.NEW
            );

            return Task.CompletedTask;
        }
    }
}