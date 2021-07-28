namespace EventHorizon.Zone.System.Backup.Events
{
    using EventHorizon.Zone.System.Backup.Model;
    using MediatR;

    public struct CreateBackupOfFileCommand
        : IRequest<BackupFileResponse>
    {
        public string FileFullName { get; }

        public CreateBackupOfFileCommand(
            string fileFullName
        )
        {
            FileFullName = fileFullName;
        }
    }
}
