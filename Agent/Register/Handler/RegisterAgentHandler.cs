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

            await _mediator.Send(new StartAgentRoutineEvent
            {
                Routine = request.Agent.Ai.DefaultRoutine,
                AgentId = registeredEntity.Id
            });

            return await _agentRepository.FindById(registeredEntity.Id);
        }
    }
}