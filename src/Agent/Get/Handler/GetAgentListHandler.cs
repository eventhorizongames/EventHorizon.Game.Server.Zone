using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Get.Handler
{
    public class GetAgentListHandler : IRequestHandler<GetAgentListEvent, IEnumerable<AgentEntity>>
    {
        readonly IAgentRepository _agentRepository;
        public GetAgentListHandler(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }
        public Task<IEnumerable<AgentEntity>> Handle(GetAgentListEvent request, CancellationToken cancellationToken)
        {
            return _agentRepository.All();
        }
    }
}