namespace EventHorizon.Zone.Core.Events.DirectoryService;

using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.DirectoryService;

using MediatR;

public struct GetListOfDirectoriesFromDirectory : IRequest<IEnumerable<StandardDirectoryInfo>>
{
    public string DirectoryFullName { get; }

    public GetListOfDirectoriesFromDirectory(
        string directoryFullName
    )
    {
        DirectoryFullName = directoryFullName;
    }
}
