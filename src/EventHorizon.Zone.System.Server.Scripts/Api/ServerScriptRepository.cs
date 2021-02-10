namespace EventHorizon.Zone.System.Server.Scripts.Api
{
    using EventHorizon.Zone.System.Server.Scripts.Model;

    public interface ServerScriptRepository
    {
        void Clear();

        void Add(
            ServerScript serverScript
        );

        ServerScript Find(
            string name
        );
    }
}