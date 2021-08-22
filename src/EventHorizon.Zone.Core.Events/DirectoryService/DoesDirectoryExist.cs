namespace EventHorizon.Zone.Core.Events.DirectoryService
{
    using MediatR;

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
