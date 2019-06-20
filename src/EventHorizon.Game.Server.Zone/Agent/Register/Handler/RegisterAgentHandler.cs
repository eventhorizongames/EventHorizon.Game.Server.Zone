using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.PopulateData;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Zone.System.Agent.Behavior.Register;

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
            // Check for already existing Agent Entity
            var agent = await CheckAndRegisterAgent(
                request.Agent
            );
            if (!agent.IsFound())
            {
                return agent;
            }

            await _mediator.Send(
                new RegisterActorWithBehaviorTreeUpdate(
                    agent.Id,
                    agent.GetProperty<AgentBehavior>(
                        AgentBehavior.PROPERTY_NAME
                    ).TreeId
                )
            );

            return agent;
        }

        private async Task<AgentEntity> CheckAndRegisterAgent(AgentEntity newAgent)
        {
            var agent = await _mediator.Send(
                new FindAgentByIdEvent(newAgent.AgentId)
            );
            if (!agent.IsFound())
            {
                // Agent was not found, register new.
                await _mediator.Publish(new PopulateAgentEntityDataEvent
                {
                    Agent = newAgent,
                });
                var registeredEntity = await _mediator.Send(new RegisterEntityEvent
                {
                    Entity = newAgent,
                });
                if (!registeredEntity.IsFound())
                {
                    return AgentEntity.CreateNotFound();
                }
                agent = await _agentRepository.FindById(registeredEntity.Id);
                if (!agent.IsFound())
                {
                    return AgentEntity.CreateNotFound();
                }
            }
            return agent;
        }
    }
}