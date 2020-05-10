namespace EventHorizon.Zone.System.Agent.Move.Queue
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Move;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.Path;
    using EventHorizon.Zone.System.Agent.Model.State;
    using MediatR;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using Microsoft.Extensions.Logging;

    public class QueueAgentToMoveHandler : IRequestHandler<QueueAgentToMove>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IAgentRepository _agentRepository;
        private readonly IMoveAgentRepository _moveRepository;

        public QueueAgentToMoveHandler(
            ILogger<QueueAgentToMoveHandler> logger,
            IMediator mediator,
            IAgentRepository agentRepository,
            IMoveAgentRepository moveRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _agentRepository = agentRepository;
            _moveRepository = moveRepository;
        }

        public async Task<Unit> Handle(
            QueueAgentToMove request,
            CancellationToken cancellationToken
        )
        {
            var agent = await _agentRepository.FindById(
                request.EntityId
            );
            var pathState = agent.GetProperty<PathState>(
                PathState.PROPERTY_NAME
            );
            pathState = pathState.SetPath(request.Path);
            pathState.MoveTo = request.MoveTo;
            agent.SetProperty(
                PathState.PROPERTY_NAME,
                pathState
            );
            
            await _mediator.Send(
                new UpdateEntityCommand(
                    AgentAction.PATH,
                    agent
                )
            );
            
            _moveRepository.Register(
                agent.Id
            );

            return Unit.Value;
        }
    }
}