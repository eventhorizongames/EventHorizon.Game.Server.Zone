using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public class AgentAction : EntityAction
    {
        public static readonly AgentAction PATH = new AgentAction("Agent.Path");
        public static readonly AgentAction ROUTINE = new AgentAction("Agent.Routine");

        protected AgentAction(string type)
            : base(type)
        {
        }
    }
}