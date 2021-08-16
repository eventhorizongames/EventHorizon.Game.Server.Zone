namespace EventHorizon.Zone.System.DataStorage.Events.Create
{
    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public struct CreateDataStoreValueCommand
        : IRequest<StandardCommandResult>
    {
        public string Key { get; }
        public string Type { get; }
        public object Value { get; }

        public CreateDataStoreValueCommand(
            string key,
            string type,
            object value
        )
        {
            Key = key;
            Type = type;
            Value = value;
        }
    }
}
