using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Agent.Model
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