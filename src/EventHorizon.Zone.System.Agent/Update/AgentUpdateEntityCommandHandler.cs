using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Update;
using EventHorizon.Zone.System.Agent.Model.State;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Update
{
    public class AgentUpdateEntityCommandHandler : IRequestHandler<AgentUpdateEntityCommand>
    {
        private readonly IAgentRepository _agentRepository;

        public AgentUpdateEntityCommandHandler(
            IAgentRepository agentRepository
        )
        {
            _agentRepository = agentRepository;
        }

        public async Task<Unit> Handle(
            AgentUpdateEntityCommand request, 
            CancellationToken cancellationToken
        )
        {
            await _agentRepository.Update(
                request.UpdateAction,
                request.Agent
            );
            return Unit.Value;
        }
    }
}