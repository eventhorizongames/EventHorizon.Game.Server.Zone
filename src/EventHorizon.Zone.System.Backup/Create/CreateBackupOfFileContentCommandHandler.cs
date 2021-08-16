namespace EventHorizon.Zone.System.Backup.Create
{
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.Backup.Model;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Text;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class CreateBackupOfFileContentCommandHandler
        : IRequestHandler<CreateBackupOfFileContentCommand, BackupFileResponse>
    {
        private static readonly DateTime JAVASCRIPT_OFFSET = new(
            1970,
            1,
            1,
            0,
            0,
            0,
            DateTimeKind.Utc
        );

        private readonly ServerInfo _serverInfo;
        private readonly IDateTimeService _dateTimeService;
        private readonly IJsonFileSaver _fileSaver;

        public CreateBackupOfFileContentCommandHandler(
            ServerInfo serverInfo,
            IDateTimeService dateTimeService,
            IJsonFileSaver fileSaver
        )
        {
            _serverInfo = serverInfo;
            _dateTimeService = dateTimeService;
            _fileSaver = fileSaver;
        }

        public async Task<BackupFileResponse> Handle(
            CreateBackupOfFileContentCommand request,
            CancellationToken cancellationToken
        )
        {
            var created = _dateTimeService.Now;
            var createdString = created
                .Subtract(
                    JAVASCRIPT_OFFSET
                )
                .TotalMilliseconds
                .ToString();
            var backupFileName = GenerateBackupFileName(
                request.FilePath,
                createdString,
                request.FileName
            );
            var backupFile = new BackupFileData(
                request.FileName,
                request.FilePath,
                request.FileContent,
                created
            );

            var backupFilePath = Path.Combine(
                _serverInfo.SystemBackupPath
            );

            await _fileSaver.SaveToFile(
                backupFilePath,
                backupFileName,
                backupFile
            );

            return new BackupFileResponse(
                backupFile
            );
        }

        /// <summary>
        /// Create FileName for backup file, <FilePath>_<CreatedDate>_<FileName>.json
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="createdString"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GenerateBackupFileName(
            IList<string> filePath,
            string createdString,
            string fileName
        )
        {
            var backupFileNameStringBuilder = new StringBuilder();
            foreach (var pathPart in filePath)
            {
                backupFileNameStringBuilder.Append(
                    pathPart
                ).Append(
                    "_"
                );
            }
            return backupFileNameStringBuilder.Append(
                fileName
            ).Append(
                "__"
            ).Append(
                createdString
            ).Append(
                "__"
            ).Append(
                ".json"
            ).ToString();
        }
    }
}
