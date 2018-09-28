using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Agent.PopulateData;
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
            var agentToRegister = await _mediator.Send(new PopulateAgentEntityDataEvent
            {
                Agent = request.Agent
            });
            var registeredEntity = await _mediator.Send(new RegisterEntityEvent
            {
                Entity = agentToRegister,
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
            agent.SetProperty("Routine", agent.GetProperty<AgentAiState>("Ai").DefaultRoutine);
            await _agentRepository.Update(AgentAction.ROUTINE, agent);
            await _mediator.Publish(new StartAgentRoutineEvent
            {
                Routine = agent.GetProperty<AgentAiState>("Ai").DefaultRoutine,
                AgentId = agent.Id
            });

            return agent;
        }
    }
}