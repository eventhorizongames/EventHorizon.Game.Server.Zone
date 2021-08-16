using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.State;

using MediatR;

namespace EventHorizon.Zone.System.Agent.Get
{
    public class GetAgentHandler : IRequestHandler<GetAgentEvent, AgentEntity>
    {
        readonly IAgentRepository _agentRepository;
        public GetAgentHandler(
            IAgentRepository agentRepository
        )
        {
            _agentRepository = agentRepository;
        }
        public Task<AgentEntity> Handle(
            GetAgentEvent request,
            CancellationToken cancellationToken
        )
        {
            return _agentRepository.FindById(
                request.EntityId
            );
        }
    }
}
