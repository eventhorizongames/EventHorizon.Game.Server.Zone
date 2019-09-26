using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Get
{
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
            return _agentRepository.All();
        }
    }
}