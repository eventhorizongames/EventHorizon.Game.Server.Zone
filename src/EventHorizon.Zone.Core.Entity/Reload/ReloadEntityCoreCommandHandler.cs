namespace EventHorizon.Zone.Core.Entity.Reload
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Api;
    using EventHorizon.Zone.Core.Entity.Load;
    using EventHorizon.Zone.Core.Events.Entity.Reload;
    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public class ReloadEntityCoreCommandHandler
        : IRequestHandler<ReloadEntityCoreCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly EntitySettingsCache _cache;

        public ReloadEntityCoreCommandHandler(
            IMediator mediator,
            EntitySettingsCache cache
        )
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<StandardCommandResult> Handle(
            ReloadEntityCoreCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new LoadEntityCoreCommand(),
                cancellationToken
            );

            if (result
                && result.WasUpdated
            )
            {
                await _mediator.Publish(
                    new EntityCoreReloadedEvent(
                        _cache.EntityConfiguration
                    ),
                    cancellationToken
                );
            }

            return new();
        }
    }
}
