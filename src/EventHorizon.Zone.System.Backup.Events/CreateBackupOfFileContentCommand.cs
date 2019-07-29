using System.Collections.Generic;
using EventHorizon.Zone.System.Backup.Model;
using MediatR;

namespace EventHorizon.Zone.System.Backup.Events
{
    public struct CreateBackupOfFileContentCommand : IRequest<BackupFileResponse>
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