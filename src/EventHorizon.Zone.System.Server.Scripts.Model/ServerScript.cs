using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public interface ServerScript
    {
        string Id { get; }
        
        Task<ServerScriptResponse> Run(
            ServerScriptServices services,
            IDictionary<string, object> data
        );
    }
}