namespace EventHorizon.Zone.System.ClientEntities.Delete
{
    using MediatR;

    public struct DeleteClientEntityCommand : IRequest<DeleteClientEntityResponse>
    {
        public string ClientEntityId { get; }

        public DeleteClientEntityCommand(
            string clientEntityId
        )
        {
            ClientEntityId = clientEntityId;
        }
    }

    public struct DeleteClientEntityResponse
    {
        public bool Success { get; }
        public string ErrorCode { get; }

        public DeleteClientEntityResponse(
            bool success
        ) : this(string.Empty)
        {
            Success = success;
        }

        public DeleteClientEntityResponse(
            string errorCode
        )
        {
            Success = false;
            ErrorCode = errorCode;
        }
    }
}
