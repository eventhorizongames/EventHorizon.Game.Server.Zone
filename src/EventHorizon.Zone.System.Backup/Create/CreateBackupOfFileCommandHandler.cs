namespace EventHorizon.Zone.System.Backup.Create
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.Backup.Model;

    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class CreateBackupOfFileCommandHandler
        : IRequestHandler<CreateBackupOfFileCommand, BackupFileResponse>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public CreateBackupOfFileCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<BackupFileResponse> Handle(
            CreateBackupOfFileCommand request,
            CancellationToken cancellationToken
        )
        {
            // Get File Name
            var fileName = Path.GetFileName(
                request.FileFullName
            );
            // Get File Path
            var filePath = _serverInfo.AppDataPath.MakePathRelative(
                Path.GetDirectoryName(
                    request.FileFullName
                ) ?? string.Empty
            ).Split(
                Path.DirectorySeparatorChar
            );
            // Get File Content
            var fileContent = await _mediator.Send(
                new ReadAllTextFromFile(
                    request.FileFullName
                ),
                cancellationToken
            );

            return await _mediator.Send(
                new CreateBackupOfFileContentCommand(
                    filePath,
                    fileName,
                    fileContent
                ),
                cancellationToken
            );
        }
    }
}
