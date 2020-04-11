namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    using global::System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Save;

    /// <summary>
    /// Make sure this stays on the ExternalHub root namespace.
    /// This Class is encapsulating the Command related logic,
    ///  and allows for a single SignalR hub to host all APIs.
    /// </summary>
    public partial class AdminHub : Hub
    {
        public Task ClientEntity_Save(
            ClientEntity clientEntity
        ) => _mediator.Send(
            new SaveClientEntityCommand(
                clientEntity
            )
        );
    }
}