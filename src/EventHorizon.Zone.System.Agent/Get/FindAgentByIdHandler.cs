using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Get
{
    public class FindAgentByIdHandler : IRequestHandler<FindAgentByIdEvent, AgentEntity>
    {
        readonly IAgentRepository _agentRepository;
        public FindAgentByIdHandler(
            IAgentRepository agentRepository
        )
        {
            _agentRepository = agentRepository;
        }
        public Task<AgentEntity> Handle(
            FindAgentByIdEvent request,
            CancellationToken cancellationToken
        )
        {
            return _agentRepository.FindByAgentId(
                request.AgentId
            );
        }
    }
}