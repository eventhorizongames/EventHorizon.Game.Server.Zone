namespace EventHorizon.Zone.Core.Events.DirectoryService;

using MediatR;

public struct CreateDirectory : IRequest<bool>
{
    public string DirectoryFullName { get; }

    public CreateDirectory(
        string directoryFullName
    )
    {
        DirectoryFullName = directoryFullName;
    }
}
