using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Model.State;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Move
{
    public class IsAgentMovingHandler : IRequestHandler<IsAgentMoving, bool>
    {
        readonly IAgentRepository _agentRepository;
        public IsAgentMovingHandler(
            IAgentRepository agentRepository
        )
        {
            _agentRepository = agentRepository;
        }
        public async Task<bool> Handle(
            IsAgentMoving request,
            CancellationToken cancellationToken
        )
        {
            return (await _agentRepository.FindById(
                request.EntityId
            )).Path?.Count > 0;
        }
    }
}