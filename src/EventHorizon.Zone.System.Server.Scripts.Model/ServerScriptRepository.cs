namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public interface ServerScriptRepository
    {
        void Add(
            ServerScript serverScript
        );
        ServerScript Find(
            string name
        );
    }
}