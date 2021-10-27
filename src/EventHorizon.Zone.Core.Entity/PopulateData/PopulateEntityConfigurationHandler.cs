namespace EventHorizon.Zone.Core.Entity.PopulateData
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Api;
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public class PopulateEntityConfigurationHandler
        : INotificationHandler<PopulateEntityDataEvent>
    {
        private readonly EntitySettingsCache _cache;

        public PopulateEntityConfigurationHandler(
            EntitySettingsCache cache
        )
        {
            _cache = cache;
        }

        public Task Handle(
            PopulateEntityDataEvent notification,
            CancellationToken cancellationToken
        )
        {
            var entity = notification.Entity;

            entity.SetProperty(
                "entityConfiguration",
                _cache.EntityConfiguration
            );

            return Task.CompletedTask;
        }
    }
}
