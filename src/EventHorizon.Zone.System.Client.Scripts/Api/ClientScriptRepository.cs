namespace EventHorizon.Zone.System.Client.Scripts.Api;

using EventHorizon.Zone.System.Client.Scripts.Model;

using global::System.Collections.Generic;

public interface ClientScriptRepository
{
    void Add(
        ClientScript clientScript
    );
    IEnumerable<ClientScript> All();
}
