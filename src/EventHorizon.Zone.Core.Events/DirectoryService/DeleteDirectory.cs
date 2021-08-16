using MediatR;

namespace EventHorizon.Zone.Core.Events.DirectoryService
{
    public struct DeleteDirectory : IRequest<bool>
    {
        public string DirectoryFullName { get; }

        public DeleteDirectory(
            string directoryFullName
        )
        {
            DirectoryFullName = directoryFullName;
        }
    }
}
