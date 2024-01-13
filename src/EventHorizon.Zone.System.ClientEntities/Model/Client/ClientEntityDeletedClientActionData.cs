namespace EventHorizon.Zone.System.ClientEntities.Model.Client;

using EventHorizon.Zone.Core.Model.Client;

public struct ClientEntityDeletedClientActionData : IClientActionData
{
    public string GlobalId { get; }

    public ClientEntityDeletedClientActionData(
        string globalId
    )
    {
        GlobalId = globalId;
    }
}
