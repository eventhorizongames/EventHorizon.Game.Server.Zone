namespace EventHorizon.Zone.System.Server.Scripts.System
{
    using EventHorizon.Zone.System.Server.Scripts.Model;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.DependencyInjection;

    public class SystemServerScriptMediator
        : ServerScriptMediator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SystemServerScriptMediator(
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default
        ) where TNotification : INotification
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Publish(
                notification,
                cancellationToken
            );
        }

        public async Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default
        )
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(
                request,
                cancellationToken
            );
        }
    }
}
