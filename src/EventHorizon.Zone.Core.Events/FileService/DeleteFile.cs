namespace EventHorizon.Zone.Core.Events.FileService;

using MediatR;

public struct DeleteFile : IRequest
{
    public string FileFullName { get; }

    public DeleteFile(
        string fileFullName
    )
    {
        FileFullName = fileFullName;
    }
}
