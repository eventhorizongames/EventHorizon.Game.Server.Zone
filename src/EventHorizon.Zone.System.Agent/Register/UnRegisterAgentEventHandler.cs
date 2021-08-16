using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Model;

using MediatR;

namespace EventHorizon.Zone.System.Agent.UnRegister
{
    public class UnRegisterAgentHandler : IRequestHandler<UnRegisterAgent>
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
                    agent.Id,
                    agent.AgentId
                )
            );

            return Unit.Value;
        }
    }
}
