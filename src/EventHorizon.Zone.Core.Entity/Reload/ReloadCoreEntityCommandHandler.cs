namespace EventHorizon.Zone.Core.Entity.Reload
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Api;
    using EventHorizon.Zone.Core.Entity.Load;
    using EventHorizon.Zone.Core.Events.Entity.Reload;
    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public class ReloadCoreEntityCommandHandler
        : IRequestHandler<ReloadCoreEntityCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly EntitySettingsCache _cache;

        public ReloadCoreEntityCommandHandler(
            IMediator mediator,
            EntitySettingsCache cache
        )
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<StandardCommandResult> Handle(
            ReloadCoreEntityCommand request,
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
                    new CoreEntityReloadedEvent(
                        _cache.EntityConfiguration
                    ),
                    cancellationToken
                );
            }

            return new();
        }
    }
}
