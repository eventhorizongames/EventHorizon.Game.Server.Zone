using System;
using System.Collections.Generic;

namespace EventHorizon.Zone.System.Backup.Model
{
    public struct BackupFileData
    {
        public string FileName { get; }
        public IList<string> FilePath { get; }
        public string FileContent { get; }
        public DateTime Created { get; }

        public BackupFileData(
            string fileName,
            IList<string> filePath,
            string fileContent,
            DateTime created
        )
        {
            FileName = fileName;
            FilePath = filePath;
            FileContent = fileContent;
            Created = created;
        }
    }
}
