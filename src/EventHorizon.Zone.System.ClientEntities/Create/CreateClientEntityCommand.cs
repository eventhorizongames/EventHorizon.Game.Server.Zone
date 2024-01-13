namespace EventHorizon.Zone.System.ClientEntities.Create;

using EventHorizon.Zone.System.ClientEntities.Model;

using MediatR;

public class CreateClientEntityCommand : IRequest<CreateClientEntityResponse>
{
    public ClientEntity ClientEntity { get; }

    public CreateClientEntityCommand(
        ClientEntity clientEntity
    )
    {
        ClientEntity = clientEntity;
    }
}

public struct CreateClientEntityResponse
{
    public bool Success { get; }
    public string ErrorCode { get; }
    public ClientEntity ClientEntity { get; }

    public CreateClientEntityResponse(
        bool success,
        ClientEntity clientEntity
    ) : this(string.Empty)
    {
        Success = success;
        ClientEntity = clientEntity;
    }

    public CreateClientEntityResponse(
        string errorCode
    )
    {
        Success = false;
        ErrorCode = errorCode;
        ClientEntity = default;
    }
}
