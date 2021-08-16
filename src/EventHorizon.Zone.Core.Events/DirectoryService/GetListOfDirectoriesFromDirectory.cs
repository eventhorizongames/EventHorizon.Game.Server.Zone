using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.DirectoryService;

using MediatR;

namespace EventHorizon.Zone.Core.Events.DirectoryService
{
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
}
