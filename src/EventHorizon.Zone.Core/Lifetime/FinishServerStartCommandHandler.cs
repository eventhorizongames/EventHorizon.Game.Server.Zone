namespace EventHorizon.Zone.Core.Lifetime
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Lifetime.State;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class FinishServerStartCommandHandler : IRequestHandler<FinishServerStartCommand, bool>
    {
        readonly IMediator _mediator;
        readonly ServerLifetimeState _serverLifetimeState;

        public FinishServerStartCommandHandler(
            IMediator mediator,
            ServerLifetimeState serverLifetimeState
        )
        {
            _mediator = mediator;
            _serverLifetimeState = serverLifetimeState;
        }

        public async Task<bool> Handle(
            FinishServerStartCommand request,
            CancellationToken cancellationToken
        )
        {
            _serverLifetimeState.SetServerStarted(
                true
            );
            await _mediator.Publish(
                new ServerFinishedStartingEvent(),
                cancellationToken
            );

            return _serverLifetimeState.IsServerStarted();
        }
    }
}