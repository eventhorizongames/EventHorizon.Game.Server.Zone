using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using MediatR;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.UnRegister;
using EventHorizon.Zone.System.Agent.Events.Get;

namespace EventHorizon.Zone.System.Agent.UnRegister
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