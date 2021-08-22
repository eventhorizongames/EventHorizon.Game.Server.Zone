namespace EventHorizon.Plugin.Zone.Interaction.PopulateData
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Interaction.Model;

    using MediatR;

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
