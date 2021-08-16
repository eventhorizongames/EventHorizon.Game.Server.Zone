namespace EventHorizon.Zone.System.ClientEntities.Register
{
    using EventHorizon.Zone.System.ClientEntities.Model;

    using MediatR;

    public struct RegisterClientEntityCommand : IRequest
    {
        public ClientEntity ClientEntity { get; }

        public RegisterClientEntityCommand(
            ClientEntity clientEntity
        )
        {
            ClientEntity = clientEntity;
        }
    }
}
