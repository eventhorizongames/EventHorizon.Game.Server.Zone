using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Model.Data;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public class AgentData : object
    {
        private dynamic _data;
        public AgentData(dynamic data)
        {
            _data = data;
            Wander = new AgentWanderData(data?.Wander);
        }
        public AiRoutine Routine
        {
            get
            {
                return _data.Routine;
            }
            set
            {
                _data.Routine = value;
            }
        }

        public AgentWanderData Wander { get; }
    }
}