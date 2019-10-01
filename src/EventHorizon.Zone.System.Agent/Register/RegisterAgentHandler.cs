using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using MediatR;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.PopulateData;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.Core.Events.Entity.Register;

namespace EventHorizon.Zone.System.Agent.Register.Handler
{
    public class RegisterAgentHandler : IRequestHandler<RegisterAgentEvent, AgentEntity>
    {
        readonly IMediator _mediator;
        readonly IAgentRepository _agentRepository;
        public RegisterAgentHandler(
            IMediator mediator,
            IAgentRepository agentRepository
        )
        {
            _mediator = mediator;
            _agentRepository = agentRepository;
        }
        public async Task<AgentEntity> Handle(
            RegisterAgentEvent request,
            CancellationToken cancellationToken
        )
        {
            // Check for already existing Agent Entity
            var agent = await CheckAndRegisterAgent(
                request.Agent
            );
            if (!agent.IsFound())
            {
                return agent;
            }

            await _mediator.Publish(
                new AgentRegisteredEvent(
                    agent.AgentId
                )
            );

            return agent;
        }

        private async Task<AgentEntity> CheckAndRegisterAgent(
            AgentEntity newAgent
        )
        {
            var agent = await _mediator.Send(
                new FindAgentByIdEvent(
                    newAgent.AgentId
                )
            );
            if (!agent.IsFound())
            {
                // Agent was not found, register new.
                await _mediator.Publish(
                    new PopulateAgentEntityDataEvent
                    {
                        Agent = newAgent,
                    }
                );
                var registeredEntity = await _mediator.Send(
                    new RegisterEntityEvent
                    {
                        Entity = newAgent,
                    }
                );
                if (!registeredEntity.IsFound())
                {
                    return AgentEntity.CreateNotFound();
                }
                agent = await _agentRepository.FindById(
                    registeredEntity.Id
                );
                if (!agent.IsFound())
                {
                    return AgentEntity.CreateNotFound();
                }
            }
            return agent;
        }
    }
}