namespace EventHorizon.Zone.System.Agent.Events.Get
{
    using EventHorizon.Zone.System.Agent.Model;

    using MediatR;

    public struct GetAgentEvent : IRequest<AgentEntity>
    {
        public long EntityId { get; set; }

        public GetAgentEvent(
            long entityId
        )
        {
            EntityId = entityId;
        }
    }
}
