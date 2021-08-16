namespace EventHorizon.Zone.System.Backup.Events
{
    using EventHorizon.Zone.System.Backup.Model;

    using global::System.Collections.Generic;

    using MediatR;

    public struct CreateBackupOfFileContentCommand
        : IRequest<BackupFileResponse>
    {
        public string FileName { get; }
        public IList<string> FilePath { get; }
        public string FileContent { get; }

        public CreateBackupOfFileContentCommand(
            IList<string> filePath,
            string fileName,
            string fileContent
        )
        {
            FilePath = filePath;
            FileName = fileName;
            FileContent = fileContent;
        }
    }
}
