namespace EventHorizon.Zone.System.Client.Scripts.Api
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Client.Scripts.Model;

    public interface ClientScriptRepository
    {
        void Add(
            ClientScript clientScript
        );
        IEnumerable<ClientScript> All();
    }
}