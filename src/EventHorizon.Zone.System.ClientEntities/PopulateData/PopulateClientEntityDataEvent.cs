namespace EventHorizon.Zone.System.ClientEntities.PopulateData;

using EventHorizon.Zone.System.ClientEntities.Model;

using MediatR;

public struct PopulateClientEntityDataEvent : INotification
{
    public ClientEntity ClientEntity { get; }

    public PopulateClientEntityDataEvent(
        ClientEntity clientEntity
    )
    {
        ClientEntity = clientEntity;
    }
}
