using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Repository
{
    public interface IMoveAgentRepository
    {
        IEnumerable<long> All();
        void Add(long entityId);
        void Remove(long entityId);
    }
}