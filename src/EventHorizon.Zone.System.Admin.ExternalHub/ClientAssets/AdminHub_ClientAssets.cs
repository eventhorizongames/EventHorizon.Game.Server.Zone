namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ClientAssets.Events.Create;
using EventHorizon.Zone.System.ClientAssets.Events.Delete;
using EventHorizon.Zone.System.ClientAssets.Events.Query;
using EventHorizon.Zone.System.ClientAssets.Events.Update;
using EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Collections.Generic;
using global::System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Make sure this stays on the ExternalHub root namespace.
/// This Class is encapsulating the Command related logic,
///  and allows for a single SignalR hub to host all APIs.
/// </summary>
public partial class AdminHub
    : Hub
{
    public Task<CommandResult<IEnumerable<ClientAsset>>> ClientAssets_All()
        => _mediator.Send(
            new QueryForAllClientAssets(),
            Context.ConnectionAborted
        );

    public Task<CommandResult<ClientAsset>> ClientAssets_Get(
        string id
    ) => _mediator.Send(
        new QueryForClientAssetById(
            id
        ),
        Context.ConnectionAborted
    );

    public Task<StandardCommandResult> ClientAssets_Create(
        ClientAsset clientAsset
    ) => _mediator.Send(
        new CreateClientAssetCommand(
            clientAsset
        ),
        Context.ConnectionAborted
    );

    public Task<StandardCommandResult> ClientAssets_Update(
        ClientAsset clientAsset
    ) => _mediator.Send(
        new UpdateClientAssetCommand(
            clientAsset
        ),
        Context.ConnectionAborted
    );

    public Task<StandardCommandResult> ClientAssets_Delete(
        string clientAssetId
    ) => _mediator.Send(
        new DeleteClientAssetCommand(
            clientAssetId
        ),
        Context.ConnectionAborted
    );
}
