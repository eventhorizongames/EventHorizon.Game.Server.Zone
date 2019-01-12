using System.Collections.Concurrent;
using EventHorizon.Game.Server.Zone.Agent.Ai.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.State
{
    public interface IAgentRoutineRepository
    {
        void Add(AgentRoutineScript script);
        AgentRoutineScript Find(string name);
    }
    public class AgentRoutineRepository : IAgentRoutineRepository
    {
        private static readonly ConcurrentDictionary<string, AgentRoutineScript> ROUTINE_MAP = new ConcurrentDictionary<string, AgentRoutineScript>();
        public AgentRoutineScript Find(string name)
        {
            var routine = default(AgentRoutineScript);
            ROUTINE_MAP.TryGetValue(name, out routine);
            return routine;
        }
        public void Add(AgentRoutineScript routine)
        {
            ROUTINE_MAP.AddOrUpdate(routine.RoutineName, routine, (key, old) => routine);
        }
    }
}