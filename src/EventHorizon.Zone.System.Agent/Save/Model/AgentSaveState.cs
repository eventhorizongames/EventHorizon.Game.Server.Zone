using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Connection.Model;

namespace EventHorizon.Zone.System.Agent.Save.Model
{
    public class AgentSaveState
    {
        public IList<AgentDetails> AgentList { get; set; }
    }
}