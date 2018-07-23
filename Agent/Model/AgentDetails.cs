using EventHorizon.Game.Server.Zone.Core.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentDetails
    {
        public string Name { get; set; }
        public PositionState Position { get; set; }

        public object Data { get; set; }
        public int Speed { get; set; }
    }
}