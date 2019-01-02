using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Handlers
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
            var agent = await _agentRepository.FindById(notification.EntityId);
            if (!agent.IsFound())
            {
                return;
            }
            agent.SetProperty(AgentRoutine.ROUTINE_NAME, notification.Routine);
            await _agentRepository.Update(AgentAction.ROUTINE, agent);
        }
    }
}