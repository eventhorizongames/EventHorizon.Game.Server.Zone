using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Register
{
    public class RegisterAgentEvent : IRequest<AgentEntity>
    {
        public AgentEntity Agent { get; set; }
    }
}