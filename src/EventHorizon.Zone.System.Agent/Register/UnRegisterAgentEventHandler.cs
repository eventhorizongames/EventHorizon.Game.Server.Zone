namespace EventHorizon.Zone.System.Agent.UnRegister
{
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Register;
    using EventHorizon.Zone.System.Agent.Model;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class UnRegisterAgentHandler : IRequestHandler<UnRegisterAgent>
    {
        readonly IMediator _mediator;

        public UnRegisterAgentHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(UnRegisterAgent request, CancellationToken cancellationToken)
        {
            var agent = await _mediator.Send(new FindAgentByIdEvent(request.AgentId));
            if (!agent.IsFound())
            {
                return;
            }

            await _mediator.Publish(new UnRegisterEntityEvent(agent));

            await _mediator.Publish(new AgentUnRegisteredEvent(agent.Id, agent.AgentId));
        }
    }
}
