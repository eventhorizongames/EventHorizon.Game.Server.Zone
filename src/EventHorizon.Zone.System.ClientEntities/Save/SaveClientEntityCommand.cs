namespace EventHorizon.Zone.System.ClientEntities.Save
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using MediatR;

    public class SaveClientEntityCommand : IRequest
    {
        public ClientEntity ClientEntity { get; }

        public SaveClientEntityCommand(
            ClientEntity clientEntity
        )
        {
            ClientEntity = clientEntity;
        }
    }
}