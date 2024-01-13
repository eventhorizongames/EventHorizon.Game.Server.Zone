namespace EventHorizon.Zone.Core.Events.FileService;

using MediatR;

public struct ReadAllTextAsBytesFromFile
    : IRequest<byte[]>
{
    public string FileFullName { get; }

    public ReadAllTextAsBytesFromFile(
        string fileFullName
    )
    {
        FileFullName = fileFullName;
    }
}
