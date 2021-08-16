namespace EventHorizon.Zone.System.ClientAssets.Events.Query
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.ClientAssets.Model;

    using MediatR;

    public struct QueryForClientAssetById
        : IRequest<CommandResult<ClientAsset>>
    {
        public string Id { get; }

        public QueryForClientAssetById(
            string id
        )
        {
            Id = id;
        }
    }
}
