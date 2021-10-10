namespace EventHorizon.Zone.System.Player.Reload
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Player.Api;
    using EventHorizon.Zone.System.Player.Client;
    using EventHorizon.Zone.System.Player.Load;
    using EventHorizon.Zone.System.Player.Model.Client;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class ReloadPlayerSystemCommandHandler
        : IRequestHandler<ReloadPlayerSystemCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly PlayerConfigurationCache _cache;

        public ReloadPlayerSystemCommandHandler(
            IMediator mediator,
            PlayerConfigurationCache cache
        )
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<StandardCommandResult> Handle(
            ReloadPlayerSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(
                new LoadSystemPlayerCommand(),
                cancellationToken
            );

            if (result
                && result.WasUpdated
            )
            {
                await _mediator.Publish(
                    ClientActionPlayerSystemReloadedToAllEvent.Create(
                        new PlayerSystemReloadedEventData(
                            _cache.PlayerConfiguration
                        )
                    ),
                    cancellationToken
                );
            }

            return new();
        }
    }
}
