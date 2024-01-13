namespace EventHorizon.Zone.System.DataStorage.Events.Update;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public class UpdateDataStoreValueCommand
    : IRequest<StandardCommandResult>
{
    public string Key { get; }
    public string Type { get; }
    public object Value { get; }

    public UpdateDataStoreValueCommand(
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
