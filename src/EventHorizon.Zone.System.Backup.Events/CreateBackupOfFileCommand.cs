namespace EventHorizon.Zone.System.Backup.Events
{
    using EventHorizon.Zone.System.Backup.Model;
    using MediatR;

    public struct CreateBackupOfFileCommand
        : IRequest<BackupFileResponse>
    {
        public string RootPath { get; }
        public string FileFullName { get; }

        public CreateBackupOfFileCommand(
            string rootPath,
            string fileFullName
        )
        {
            RootPath = rootPath;
            FileFullName = fileFullName;
        }
    }
}
