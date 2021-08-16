using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Register
{
    public struct AgentRegisteredEvent : INotification
    {
        public string AgentId { get; }

        public AgentRegisteredEvent(
            string agentId
        )
        {
            AgentId = agentId;
        }
    }
}
