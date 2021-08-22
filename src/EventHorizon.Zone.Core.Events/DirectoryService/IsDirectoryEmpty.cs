namespace EventHorizon.Zone.Core.Events.DirectoryService
{
    using MediatR;

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
