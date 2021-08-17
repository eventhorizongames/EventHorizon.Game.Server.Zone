namespace EventHorizon.Zone.System.Agent.Save.Model
{
    using EventHorizon.Zone.System.Agent.Connection.Model;

    using global::System.Collections.Generic;

    public class AgentSaveState
    {
        public IList<AgentDetails> AgentList { get; set; } = new List<AgentDetails>();
    }
}
