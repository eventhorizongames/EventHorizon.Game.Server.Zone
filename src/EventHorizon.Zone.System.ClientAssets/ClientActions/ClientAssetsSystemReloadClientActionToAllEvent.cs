namespace EventHorizon.Zone.System.ClientAssets.ClientActions;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Collections.Generic;

public static class ClientAssetsSystemReloadClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        IEnumerable<ClientAsset> clientAssetList
    ) =>
        new(
            "CLIENT_ASSETS_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
            new ClientAssetsSystemReloadedClientActionData(
                clientAssetList
            )
        );

    public record ClientAssetsSystemReloadedClientActionData(
        IEnumerable<ClientAsset> ClientAssetList
    ) : IClientActionData;
}
