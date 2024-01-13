namespace EventHorizon.Zone.System.Server.Scripts.Api;

using EventHorizon.Zone.System.Server.Scripts.Model;

using global::System.Collections.Generic;

public interface ServerScriptRepository
{
    IEnumerable<ServerScript> All { get; }

    void Clear();

    void Add(
        ServerScript serverScript
    );

    ServerScript Find(
        string name
    );
}
