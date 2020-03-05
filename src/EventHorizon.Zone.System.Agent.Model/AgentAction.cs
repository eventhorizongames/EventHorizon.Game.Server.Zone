using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Agent.Model
{
    public class AgentAction : EntityAction
    {
        public static readonly AgentAction PATH = new AgentAction("Agent.Path");
        public static readonly AgentAction SCRIPT = new AgentAction("Agent.Script");

        protected AgentAction(string type)
            : base(type)
        {
        }
    }
}