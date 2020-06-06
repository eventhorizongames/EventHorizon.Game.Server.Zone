namespace EventHorizon.Game.PopulateData
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Game.Model;

    public class PopulatePlayerGameDataHandler 
        : INotificationHandler<PopulateEntityDataEvent>
    {
        public Task Handle(
            PopulateEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var entity = request.Entity;

            if (entity.Type != EntityType.PLAYER)
            {
                return Task.CompletedTask;
            }

            // Populate the Game State on the Player.
            entity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                GamePlayerCaptureState.New()
            );

            return Task.CompletedTask;
        }
    }
}