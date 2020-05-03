namespace EventHorizon.Zone.System.ClientEntities.Update
{
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;

    public struct SetClientEntityNodeDensity : IRequest
    {
        public IObjectEntity ClientEntity { get; }

        public SetClientEntityNodeDensity(
            IObjectEntity clientEntity
        )
        {
            ClientEntity = clientEntity;
        }
    }
}
