using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Backup.Model;
using MediatR;

namespace EventHorizon.Zone.System.Backup.Create
{
    public struct CreateBackupOfFileContentCommandHandler : IRequestHandler<CreateBackupOfFileContentCommand, BackupFileResponse>
    {
        private static DateTime JAVASCRIPT_OFFSET = new DateTime(
            1970,
            1,
            1,
            0,
            0,
            0,
            DateTimeKind.Utc
        );
        readonly ServerInfo _serverInfo;
        readonly IDateTimeService _dateTimeService;
        readonly IJsonFileSaver _fileSaver;
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