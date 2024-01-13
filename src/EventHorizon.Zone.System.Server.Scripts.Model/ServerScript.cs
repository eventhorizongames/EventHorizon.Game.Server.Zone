namespace EventHorizon.Zone.System.Server.Scripts.Model;

using global::System.Collections.Generic;
using global::System.Threading.Tasks;

public interface ServerScript
{
    string Id { get; }
    IEnumerable<string> Tags { get; }

    Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    );
}
