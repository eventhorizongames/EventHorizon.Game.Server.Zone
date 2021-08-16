namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    using EventHorizon.Game.Server.Zone.Info.Query;

    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Make sure this stays on the ExternalHub root namespace.
    /// This Class is encapsulating the Zone Info related logic,
    ///  and allows for a single SignalR hub to host all APIs.
    /// </summary>
    public partial class AdminHub : Hub
    {
        public Task<IDictionary<string, object>> ZoneInfo() => _mediator.Send(
            new QueryForFullZoneInfo(),
            Context.ConnectionAborted
        );
    }
}
