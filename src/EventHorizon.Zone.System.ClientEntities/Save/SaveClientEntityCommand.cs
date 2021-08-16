namespace EventHorizon.Zone.System.ClientEntities.Save
{
    using EventHorizon.Zone.System.ClientEntities.Model;

    using MediatR;

    public class SaveClientEntityCommand : IRequest<SaveClientEntityResponse>
    {
        public ClientEntity ClientEntity { get; }

        public SaveClientEntityCommand(
            ClientEntity clientEntity
        )
        {
            ClientEntity = clientEntity;
        }
    }

    public struct SaveClientEntityResponse
    {
        public bool Success { get; }
        public string ErrorCode { get; }
        public ClientEntity ClientEntity { get; }

        public SaveClientEntityResponse(
            bool success,
            ClientEntity clientEntity
        ) : this(string.Empty)
        {
            Success = success;
            ClientEntity = clientEntity;
        }

        public SaveClientEntityResponse(
            string errorCode
        )
        {
            Success = false;
            ErrorCode = errorCode;
            ClientEntity = default;
        }
    }
}
