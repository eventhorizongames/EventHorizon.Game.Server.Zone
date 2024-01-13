namespace EventHorizon.Zone.Core.Events.FileService;

using MediatR;

public struct DoesFileExist : IRequest<bool>
{
    public string FileFullName { get; }

    public DoesFileExist(
        string fileFullName
    )
    {
        FileFullName = fileFullName;
    }
}
