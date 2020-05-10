namespace EventHorizon.Zone.System.Agent.Events.Register
{
    using EventHorizon.Zone.System.Agent.Model;
    using MediatR;

    public struct RegisterAgentEvent : IRequest<AgentEntity>
    {
        public AgentEntity Agent { get; }

        public RegisterAgentEvent(
            AgentEntity agent
        )
        {
            Agent = agent;
        }
    }
}