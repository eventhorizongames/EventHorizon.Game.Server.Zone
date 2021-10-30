namespace EventHorizon.Zone.System.Player.PopulateData
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Entity.Data;

    using MediatR;

    using Microsoft.Extensions.Logging;
    using EventHorizon.Zone.System.Player.Set;
    using EventHorizon.Zone.Core.Model.Player;

    public class PrePopulateEntityOverrideDataEventHandler
        : INotificationHandler<PrePopulateEntityDataEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public PrePopulateEntityOverrideDataEventHandler(
            ILogger<PrePopulateEntityOverrideDataEventHandler> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Handle(
            PrePopulateEntityDataEvent notification,
            CancellationToken cancellationToken
        )
        {
            if (notification.Entity is not PlayerEntity playerEntity)
            {
                return;
            }

            var result = await _mediator.Send(
                new SetPlayerPropertyOverrideDataCommand(
                    playerEntity
                ),
                cancellationToken
            );

            if (!result)
            {
                _logger.LogWarning(
                    "Failed to override Player Property Data: {ErrorCode}",
                    result.ErrorCode
                );
            }
        }
    }
}
