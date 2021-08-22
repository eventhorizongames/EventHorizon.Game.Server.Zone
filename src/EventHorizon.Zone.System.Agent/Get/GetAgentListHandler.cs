namespace EventHorizon.Zone.System.Agent.Get
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.State;

    using MediatR;

    public class GetAgentListHandler : IRequestHandler<GetAgentListEvent, IEnumerable<AgentEntity>>
    {
        readonly IAgentRepository _agentRepository;
        public GetAgentListHandler(
            IAgentRepository agentRepository
        )
        {
            _agentRepository = agentRepository;
        }
        public Task<IEnumerable<AgentEntity>> Handle(
            GetAgentListEvent request,
            CancellationToken cancellationToken
        )
        {
            return (request.Query != null)
                ? _agentRepository.Where(
                    request.Query
                ) : _agentRepository.All();
        }
    }
}
