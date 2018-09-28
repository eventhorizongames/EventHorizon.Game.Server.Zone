using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General.Handler
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
            if (!agent.IsFound())
            {
                return;
            }
            agent.SetProperty("Routine", notification.Routine);
            await _agentRepository.Update(AgentAction.ROUTINE, agent);
        }
    }
}