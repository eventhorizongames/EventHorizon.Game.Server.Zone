namespace EventHorizon.Zone.System.Watcher.Events.Start
{
    using MediatR;

    public struct StartWatchingFileSystemCommand : IRequest<Unit>
    {
        public string Path { get; }

        public StartWatchingFileSystemCommand(
            string path
        )
        {
            Path = path;
        }
    }
}
