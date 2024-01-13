namespace EventHorizon.Zone.Core.Events.Entity.Register;

using EventHorizon.Zone.Core.Model.Entity;

using MediatR;

public struct RegisterEntityEvent
    : IRequest<IObjectEntity>
{
    public IObjectEntity Entity { get; set; }

    public RegisterEntityEvent(
        IObjectEntity entity
    )
    {
        Entity = entity;
    }
}
