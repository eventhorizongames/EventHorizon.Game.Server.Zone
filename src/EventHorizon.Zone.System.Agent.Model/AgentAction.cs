namespace EventHorizon.Zone.System.Agent.Model
{
    using EventHorizon.Zone.Core.Model.Entity;

    public class AgentAction : EntityAction
    {
        public static readonly AgentAction PATH = new("Agent.Path");
        public static readonly AgentAction SCRIPT = new("Agent.Script");

        protected AgentAction(string type)
            : base(type)
        {
        }
    }
}
