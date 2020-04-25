namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    using EventHorizon.Zone.System.ClientEntities.Create;
    using EventHorizon.Zone.System.ClientEntities.Delete;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Save;
    using global::System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Make sure this stays on the ExternalHub root namespace.
    /// This Class is encapsulating the Command related logic,
    ///  and allows for a single SignalR hub to host all APIs.
    /// </summary>
    public partial class AdminHub : Hub
    {
        public Task<SaveClientEntityResponse> ClientEntity_Save(
            ClientEntity clientEntity
        ) => _mediator.Send(
            new SaveClientEntityCommand(
                clientEntity
            )
        );
        public Task<CreateClientEntityResponse> ClientEntity_Create(
            ClientEntity clientEntity
        ) => _mediator.Send(
            new CreateClientEntityCommand(
                clientEntity
            )
        );

        public Task<DeleteClientEntityResponse> ClientEntity_Delete(
            string clientEntityId
        ) => _mediator.Send(
            new DeleteClientEntityCommand(
                clientEntityId
            )
        );
    }
}