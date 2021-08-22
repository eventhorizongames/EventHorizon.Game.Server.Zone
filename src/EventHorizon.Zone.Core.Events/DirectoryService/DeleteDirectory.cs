namespace EventHorizon.Zone.Core.Events.DirectoryService
{
    using MediatR;

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
