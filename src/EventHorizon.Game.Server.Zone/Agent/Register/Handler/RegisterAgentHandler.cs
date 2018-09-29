using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Events;
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
            var agentToRegister = request.Agent;
            await _mediator.Publish(new PopulateAgentEntityDataEvent
            {
                Agent = agentToRegister,
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
            var defaultRoutine = agent.GetProperty<AgentRoutine>("DefaultRoutine");
            agent.SetProperty("Routine", defaultRoutine);
            await _agentRepository.Update(AgentAction.ROUTINE, agent);
            await _mediator.Publish(new ClearAgentRoutineEvent
            {
                AgentId = agent.Id
            });
            await _mediator.Publish(new StartAgentRoutineEvent
            {
                Routine = defaultRoutine,
                AgentId = agent.Id
            });

            return agent;
        }
    }
}