namespace EventHorizon.Zone.System.ClientEntities.Save
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using MediatR;

    public class SaveClientEntityCommand : IRequest
    {
        public ClientEntityDetails ClientEntity { get; }

        public SaveClientEntityCommand(
            ClientEntityDetails clientEntity
        )
        {
            ClientEntity = clientEntity;
        }
    }
}