namespace EventHorizon.Zone.Core.Lifetime
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Lifetime.State;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class IsServerStartedHandler : IRequestHandler<IsServerStarted, bool>
    {
        readonly ServerLifetimeState _serverLifetimeState;

        public IsServerStartedHandler(
            ServerLifetimeState serverLifetimeState
        )
        {
            _serverLifetimeState = serverLifetimeState;
        }

        public Task<bool> Handle(
            IsServerStarted request,
            CancellationToken cancellationToken
        ) => _serverLifetimeState
            .IsServerStarted()
            .FromResult();
    }
}