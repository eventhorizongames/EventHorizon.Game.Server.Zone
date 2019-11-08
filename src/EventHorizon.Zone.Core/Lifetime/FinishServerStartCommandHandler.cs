using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Lifetime.State;
using MediatR;

namespace EventHorizon.Zone.Core.Lifetime
{
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
                new ServerFinishedStartingEvent()
            );

            return _serverLifetimeState.IsServerStarted();
        }
    }
}