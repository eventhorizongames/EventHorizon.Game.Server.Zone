namespace EventHorizon.Zone.System.Interaction.Model.Client;

using EventHorizon.Zone.Core.Model.Client;

public struct InteractionClientActionData : IClientActionData
{
    public string CommandType { get; }
    public object Data { get; }

    public InteractionClientActionData(
        string commandType,
        object data
    )
    {
        CommandType = commandType;
        Data = data;
    }
}
