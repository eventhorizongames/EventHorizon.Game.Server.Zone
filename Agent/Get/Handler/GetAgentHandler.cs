using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Get.Handler
{
    public class GetAgentHandler : IRequestHandler<GetAgentEvent, AgentEntity>
    {
        readonly IAgentRepository _agentRepository;
        public GetAgentHandler(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }
        public Task<AgentEntity> Handle(GetAgentEvent request, CancellationToken cancellationToken)
        {
            return _agentRepository.FindById(request.AgentId);
        }
    }
}