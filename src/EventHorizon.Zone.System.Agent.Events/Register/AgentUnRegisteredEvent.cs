using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Register
{
    public struct AgentUnRegisteredEvent : INotification
    {
        public string AgentId { get; }

        public AgentUnRegisteredEvent(
            string agentId
        )
        {
            AgentId = agentId;
        }
    }
}