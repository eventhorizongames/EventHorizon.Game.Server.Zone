using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Zone.System.Agent.Behavior.Register;
using MediatR;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Zone.System.Agent.Behavior.UnRegister;

namespace EventHorizon.Game.Server.Zone.Agent.UnRegister
{
    public struct UnRegisterAgent : IRequest
    {
        public string AgentId { get; }
        public UnRegisterAgent(
            string agentId
        )
        {
            this.AgentId = agentId;
        }
        public struct UnRegisterAgentHandler : IRequestHandler<UnRegisterAgent>
        {
            readonly IMediator _mediator;
            public UnRegisterAgentHandler(
                IMediator mediator 
            )
            {
                _mediator = mediator;
            }
            public async Task<Unit> Handle(
                UnRegisterAgent request, 
                CancellationToken cancellationToken
            )
            {
                var agent = await _mediator.Send(
                    new FindAgentByIdEvent(
                        request.AgentId
                    )
                );
                if (!agent.IsFound())
                {
                    return Unit.Value;
                }

                await _mediator.Send(
                    new UnRegisterActorWithBehaviorTreeUpdate(
                        agent.Id,
                        agent.GetProperty<AgentBehavior>(
                            AgentBehavior.PROPERTY_NAME
                        ).TreeId
                    )
                );

                await _mediator.Publish(
                    new UnregisterEntityEvent
                    {
                        Entity = agent,
                    }
                );

                return Unit.Value;
            }
        }
    }
}