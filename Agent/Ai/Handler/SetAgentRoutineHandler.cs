using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Handler
{
    public class SetAgentRoutineHandler : INotificationHandler<SetAgentRoutineEvent>
    {
        readonly IAgentRepository _agentRepository;
        public SetAgentRoutineHandler(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }
        public async Task Handle(SetAgentRoutineEvent notification, CancellationToken cancellationToken)
        {
            var agent = await _agentRepository.FindById(notification.AgentId);
            agent.TypedData.Routine = notification.Routine;
            await _agentRepository.Update(agent);
        }
    }
}