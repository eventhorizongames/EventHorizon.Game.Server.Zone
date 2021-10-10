namespace EventHorizon.Zone.System.Player.PopulateData
{
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Api;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class PopulatePlayerEntityConfigurationHandler
        : INotificationHandler<PopulateEntityDataEvent>
    {
        private readonly PlayerConfigurationCache _cache;

        public PopulatePlayerEntityConfigurationHandler(
            PlayerConfigurationCache cache
        )
        {
            _cache = cache;
        }

        public Task Handle(
            PopulateEntityDataEvent notification,
            CancellationToken cancellationToken
        )
        {
            if (notification.Entity is not PlayerEntity player)
            {
                return Task.CompletedTask;
            }

            player.SetProperty(
                "playerConfiguration",
                _cache.PlayerConfiguration
            );

            return Task.CompletedTask;
        }
    }
}
