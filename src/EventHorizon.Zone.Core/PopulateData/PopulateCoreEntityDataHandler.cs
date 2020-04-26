namespace EventHorizon.Zone.Core.PopulateData
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity.Movement;
    using EventHorizon.Zone.Core.Model.Core;

    public class PopulateCoreEntityDataHandler : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(
            PopulateEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var entity = request.Entity;

            // Populates the Movement State on the Entity from loaded data.
            entity.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME,
                LocationState.NEW
            );

            // Populates the Movement State on the Entity from loaded data.
            entity.PopulateData<MovementState>(
                MovementState.PROPERTY_NAME,
                MovementState.NEW
            );

            return Task.CompletedTask;
        }
    }
}