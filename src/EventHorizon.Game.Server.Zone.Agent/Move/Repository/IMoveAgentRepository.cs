using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Repository
{
    public interface IMoveAgentRepository
    {
        void Register(long entityId);
        bool Dequeue(out long entityId);
        void MergeRegisteredIntoQueue();
    }
}