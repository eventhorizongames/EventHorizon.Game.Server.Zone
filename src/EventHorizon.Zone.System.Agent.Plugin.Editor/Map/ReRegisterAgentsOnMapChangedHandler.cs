namespace EventHorizon.Zone.System.Agent.Plugin.Editor.Map
{
    using MediatR;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using global::System.Threading;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Register;

    public class ReRegisterAgentsOnMapCreatedHandler : INotificationHandler<MapCreatedEvent>
    {
        private readonly IMediator _mediator;

        public ReRegisterAgentsOnMapCreatedHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            MapCreatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            // UnRegister Only Agents that are not Global
            var agentList = await _mediator.Send(
                new GetAgentListEvent(
                    agent => true
                )
            );
            foreach (var agent in agentList)
            {
                await _mediator.Send(
                    new UnRegisterAgent(
                        agent.AgentId
                    )
                );
                await _mediator.Send(
                    new RegisterAgentEvent(
                        agent
                    )
                );
            }
        }
    }
}