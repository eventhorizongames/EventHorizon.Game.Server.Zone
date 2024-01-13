namespace EventHorizon.Zone.Core.Events.FileService;

using MediatR;

public struct CreateFile : IRequest<bool>
{
    public string FileFullName { get; }

    public CreateFile(
        string fileFullName
    )
    {
        FileFullName = fileFullName;
    }
}
