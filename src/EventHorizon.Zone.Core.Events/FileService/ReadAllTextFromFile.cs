namespace EventHorizon.Zone.Core.Events.FileService;

using MediatR;

public struct ReadAllTextFromFile : IRequest<string>
{
    public string FileFullName { get; }

    public ReadAllTextFromFile(
        string fileFullName
    )
    {
        FileFullName = fileFullName;
    }
}
