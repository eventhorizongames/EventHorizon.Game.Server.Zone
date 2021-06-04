namespace EventHorizon.Zone.System.DataStorage.Events.Delete
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct DeleteDataStoreValueCommand
        : IRequest<StandardCommandResult>
    {
        public string Key { get; }

        public DeleteDataStoreValueCommand(
            string key
        )
        {
            Key = key;
        }
    }
}
