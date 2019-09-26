using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Agent.Model
{
    public class AgentAction : EntityAction
    {
        public static readonly AgentAction PATH = new AgentAction("Agent.Path");

        protected AgentAction(string type)
            : base(type)
        {
        }
    }
}