using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Register.Handler
{
    public class RegisterAgentHandler : IRequestHandler<RegisterAgentEvent, AgentEntity>
    {
        readonly IMediator _mediator;
        readonly IAgentRepository _agentRepository;
        public RegisterAgentHandler(IMediator mediator, IAgentRepository agentRepository)
        {
            _mediator = mediator;
            _agentRepository = agentRepository;
        }
        public async Task<AgentEntity> Handle(RegisterAgentEvent request, CancellationToken cancellationToken)
        {
            var registeredEntity = await _mediator.Send(new RegisterEntityEvent
            {
                Entity = request.Agent,
            });
            if (!registeredEntity.IsFound())
            {
                return AgentEntity.CreateNotFound();
            }

            var agent = await _agentRepository.FindById(registeredEntity.Id);
            if (!agent.IsFound())
            {
                return AgentEntity.CreateNotFound();
            }
            agent.TypedData.Routine = agent.Ai.DefaultRoutine;
            await _agentRepository.Update(AgentAction.ROUTINE, agent);
            await _mediator.Publish(new StartAgentRoutineEvent
            {
                Routine = agent.Ai.DefaultRoutine,
                AgentId = agent.Id
            });

            return agent;
        }
    }
}