using System.Dynamic;

namespace EventHorizon.Game.Server.Zone.Agent.Model.Data
{
    public class AgentWanderData
    {
        private dynamic _data;
        public AgentWanderData(dynamic data)
        {
            _data = data ?? new ExpandoObject();
        }
        public int LookDistance
        {
            get
            {
                return _data.LookDistance;
            }
            set
            {
                _data.LookDistance = value;
            }
        }
    }
}