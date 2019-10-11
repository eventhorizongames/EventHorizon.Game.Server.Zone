using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using MediatR;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.System.Agent.Events.Register;

namespace EventHorizon.Zone.System.Agent.UnRegister
{
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

            await _mediator.Publish(
                new UnRegisterEntityEvent(
                    agent
                )
            );

            await _mediator.Publish(
                new AgentUnRegisteredEvent(
                    agent.AgentId
                )
            );

            return Unit.Value;
        }
    }
}