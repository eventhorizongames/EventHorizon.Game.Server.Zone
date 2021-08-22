namespace EventHorizon.Zone.System.Agent.Plugin.Move.Events
{
    using MediatR;

    public struct AgentFinishedMoveEvent : INotification
    {
        public long EntityId { get; set; }
    }
}
