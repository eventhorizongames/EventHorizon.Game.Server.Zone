namespace EventHorizon.Zone.Core.Events.DirectoryService;

using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public struct GetListOfFilesFromDirectory : IRequest<IEnumerable<StandardFileInfo>>
{
    public string DirectoryFullName { get; }

    public GetListOfFilesFromDirectory(
        string directoryFullName
    )
    {
        DirectoryFullName = directoryFullName;
    }
}
