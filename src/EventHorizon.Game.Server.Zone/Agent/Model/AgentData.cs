using EventHorizon.Game.Server.Zone.Agent.Ai;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public class AgentData : object
    {
        private dynamic _data;
        public AgentData(dynamic data)
        {
            _data = data;
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

    }
}