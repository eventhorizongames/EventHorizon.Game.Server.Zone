namespace EventHorizon.Zone.Core.Lifetime
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Lifetime.State;
    using MediatR;

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