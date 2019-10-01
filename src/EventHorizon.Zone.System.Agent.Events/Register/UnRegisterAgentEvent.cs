using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Register
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
    }
}