namespace EventHorizon.Zone.System.Agent.Model.State
{
    public interface IMoveAgentRepository
    {
        void Register(long entityId);
        bool Dequeue(out long entityId);
        void MergeRegisteredIntoQueue();
    }
}