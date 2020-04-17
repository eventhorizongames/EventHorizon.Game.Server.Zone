namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Register;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using MediatR;

    public class AgentRegisteredEventHandler : INotificationHandler<AgentRegisteredEvent>
    {
        readonly IMediator _mediator;
        public AgentRegisteredEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            AgentRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            var agent = await _mediator.Send(
                new FindAgentByIdEvent(
                    notification.AgentId
                )
            );

            if (!agent.IsFound())
            {
                return;
            }

            await _mediator.Send(
                new ChangeActorBehaviorTreeCommand(
                    agent,
                    agent.GetProperty<AgentBehavior>(
                        AgentBehavior.PROPERTY_NAME
                    ).TreeId
                )
            );
        }
    }
}