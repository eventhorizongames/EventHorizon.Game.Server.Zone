using MediatR;

namespace EventHorizon.Zone.Core.Events.DirectoryService
{
    public struct DoesDirectoryExist : IRequest<bool>
    {
        public string DirectoryFullName { get; }

        public DoesDirectoryExist(
            string directoryFullName
        )
        {
            DirectoryFullName = directoryFullName;
        }
    }
}
