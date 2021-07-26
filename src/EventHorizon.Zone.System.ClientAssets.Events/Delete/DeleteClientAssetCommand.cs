namespace EventHorizon.Zone.System.ClientAssets.Events.Delete
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct DeleteClientAssetCommand
        : IRequest<StandardCommandResult>
    {
        public string Id { get; }

        public DeleteClientAssetCommand(
            string id
        )
        {
            Id = id;
        }
    }
}
