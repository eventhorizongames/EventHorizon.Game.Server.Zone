namespace EventHorizon.Server.Core.Check
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Server.Core.Events.Check;
    using EventHorizon.Server.Core.Events.Register;
    using EventHorizon.Server.Core.Events.Stop;
    using EventHorizon.Server.Core.State;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class CheckCoreServerConnectionHandler : INotificationHandler<CheckCoreServerConnection>
    {
        private static readonly int MAX_RETRIES = 3;

        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerCoreCheckState _serverCoreCheckState;

        public CheckCoreServerConnectionHandler(
            ILogger<CheckCoreServerConnectionHandler> logger,
            IMediator mediator,
            ServerCoreCheckState serverCoreCheckState
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverCoreCheckState = serverCoreCheckState;
        }

        public async Task Handle(
            CheckCoreServerConnection notification,
            CancellationToken cancellationToken
        )
        {
            var regsitered = await _mediator.Send(
                new QueryForRegistrationWithCoreServer()
            );
            if (!regsitered)
            {
                _logger.LogWarning(
                    "Not Registered with Core"
                );

                // Register with the server
                await _mediator.Publish(
                    new RegisterWithCoreServer()
                );
                // Reset Check
                _serverCoreCheckState.Reset();
                return;
            }
            var checksDone = _serverCoreCheckState.TimesChecked();
            if (checksDone >= MAX_RETRIES)
            {
                _logger.LogWarning(
                    "Reached {MaxRetries} retries, Restarting connection.",
                    MAX_RETRIES
                );

                await _mediator.Publish(
                    new StopCoreServerConnection()
                );
                await _mediator.Publish(
                    new RegisterWithCoreServer()
                );
                _serverCoreCheckState.Reset();
                return;
            }

            _serverCoreCheckState.Check();
        }
    }
}
