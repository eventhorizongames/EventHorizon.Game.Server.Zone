namespace EventHorizon.Zone.System.Agent.Events.Register
{
    using MediatR;

    public struct UnRegisterAgent : IRequest
    {
        public string AgentId { get; }
        public UnRegisterAgent(
            string agentId
        )
        {
            AgentId = agentId;
        }
    }
}
