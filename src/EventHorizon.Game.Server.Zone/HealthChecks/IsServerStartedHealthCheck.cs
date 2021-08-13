namespace EventHorizon.Game.Server.Zone.HealthChecks
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public class IsServerStartedHealthCheck
        : IHealthCheck
    {
        private readonly IMediator _mediator;

        public IsServerStartedHealthCheck(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            var isServerStarted = await _mediator.Send(
                new IsServerStarted(),
                cancellationToken
            );

            if (isServerStarted)
            {
                return HealthCheckResult.Healthy(
                    "Server Started."
                );
            }

            return HealthCheckResult.Unhealthy(
                "Server not Started."
            );
        }
    }
}
