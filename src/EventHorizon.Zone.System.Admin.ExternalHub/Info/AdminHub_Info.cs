using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Info.Query;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    /// <summary>
    /// Make sure this stays on the ExternalHub root namespace.
    /// This Class is encapsulating the Zone Info related logic,
    ///  and allows for a single SignalR hub to host all APIs.
    /// </summary>
    public partial class AdminHub : Hub
    {
        public Task<IDictionary<string, object>> ZoneInfo() => _mediator.Send(
            new QueryForFullZoneInfo()
        );
    }
}