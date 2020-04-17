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

    public class QueueAgentToMoveHandler : IRequestHandler<QueueAgentToMove>
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IMoveAgentRepository _moveRepository;

        public QueueAgentToMoveHandler(
            IAgentRepository agentRepository,
            IMoveAgentRepository moveRepository
        )
        {
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
            pathState.Path = request.Path;
            pathState.MoveTo = request.MoveTo;
            agent.SetProperty(
                PathState.PROPERTY_NAME,
                pathState
            );
            await _agentRepository.Update(
                AgentAction.PATH,
                agent
            );
            _moveRepository.Register(
                agent.Id
            );

            return Unit.Value;
        }
    }
}