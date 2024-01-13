namespace EventHorizon.Zone.System.Player.ExternalHub;

using EventHorizon.Zone.System.Client.Scripts.Events.Query;
using EventHorizon.Zone.System.Client.Scripts.Model.Query;

using global::System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

public partial class PlayerHub
    : Hub
{
    public Task<ClientScriptsAssemblyResult> GetClientScriptAssembly()
    {
        return _mediator.Send(
            new QueryForClientScriptsAssembly()
        );
    }

}
