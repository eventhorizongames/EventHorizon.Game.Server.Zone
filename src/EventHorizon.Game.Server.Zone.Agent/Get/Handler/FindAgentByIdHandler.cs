using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Get.Handler
{
    public struct FindAgentByIdHandler : IRequestHandler<FindAgentByIdEvent, AgentEntity>
    {
        readonly IAgentRepository _agentRepository;
        public FindAgentByIdHandler(
            IAgentRepository agentRepository
        )
        {
            _agentRepository = agentRepository;
        }
        public Task<AgentEntity> Handle(FindAgentByIdEvent request, CancellationToken cancellationToken)
        {
            return _agentRepository.FindByAgentId(
                request.AgentId
            );
        }
    }
}