namespace EventHorizon.Zone.System.Agent.Monitor.Path
{
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Move;
    using EventHorizon.Zone.System.Agent.Model.Path;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class CheckForStaleAgentPathHandler
        : INotificationHandler<CheckForStaleAgentPath>
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTime;

        public CheckForStaleAgentPathHandler(
            IMediator mediator,
            IDateTimeService dateTimeService
        )
        {
            _mediator = mediator;
            _dateTime = dateTimeService;
        }

        public async Task Handle(
            CheckForStaleAgentPath notification,
            CancellationToken cancellationToken
        )
        {
            var agentList = await _mediator.Send(
                new GetAgentListEvent(
                    agent => agent.GetProperty<LocationState>(
                        LocationState.PROPERTY_NAME
                    ).CanMove
                    && agent.GetProperty<LocationState>(
                        LocationState.PROPERTY_NAME
                    ).NextMoveRequest.CompareTo(
                        _dateTime.Now.AddSeconds(-15)
                    ) < 0
                    && agent.GetProperty<PathState>(
                        PathState.PROPERTY_NAME
                    ).Path()?.Count > 0
                )
            );
            foreach (var agent in agentList)
            {
                var pathState = agent.GetProperty<PathState>(
                    PathState.PROPERTY_NAME
                );
                var path = pathState.Path();
                if (path.IsNull()
                    || !pathState.MoveTo.HasValue
                )
                {
                    continue;
                }

                await _mediator.Send(
                    new QueueAgentToMove(
                        agent.Id,
                        path,
                        pathState.MoveTo.Value
                    )
                );
            }
        }
    }
}
