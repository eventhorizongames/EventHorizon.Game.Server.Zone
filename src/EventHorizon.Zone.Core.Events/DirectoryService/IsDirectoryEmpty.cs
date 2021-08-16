using MediatR;

namespace EventHorizon.Zone.Core.Events.DirectoryService
{
    public struct IsDirectoryEmpty : IRequest<bool>
    {
        public string DirectoryFullName { get; }

        public IsDirectoryEmpty(
            string directoryFullName
        )
        {
            DirectoryFullName = directoryFullName;
        }
    }
}
