namespace EventHorizon.Zone.Core.Plugin.LocalFileSystem
{
    using System.IO;

    using EventHorizon.Zone.Core.Model.FileService;

    public class LocalFileSystemFileResolver
        : FileResolver
    {
        public bool CreateFile(
            string fileFullName
        )
        {
            var fileInfo = new FileInfo(
               fileFullName
            );
            if (fileInfo.Directory == null)
            {
                return false;
            }

            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            using (fileInfo.Create())
            {
                return fileInfo.Exists;
            }
        }

        public StandardFileInfo GetFileInfo(
            string fileFullName
        )
        {
            var fileInfo = new FileInfo(
                fileFullName
            );
            return new StandardFileInfo(
                fileInfo.Name,
                fileInfo.DirectoryName ?? string.Empty,
                fileInfo.FullName,
                fileInfo.Extension
            );
        }

        public bool AppendTextToFile(
            string fileFullName,
            string text
        )
        {
            var fileInfo = new FileInfo(
               fileFullName
            );
            if (DoesFileExist(
                fileFullName
            ))
            {
#pragma warning disable IDE0063 // Use simple 'using' statement
                using (var writer = fileInfo.AppendText())
#pragma warning restore IDE0063 // Use simple 'using' statement
                {
                    writer.Write(
                        text
                    );
                }
            }
            else
            {
                if (fileInfo.Directory == null)
                {
                    return false;
                }

                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                WriteAllText(
                    fileFullName,
                    text
                );
            }
            return fileInfo.Exists;
        }

        public void DeleteFile(
            string fileFullName
        ) => File.Delete(
            fileFullName
        );

        public bool DoesFileExist(
            string fileFullName
        ) => File.Exists(
            fileFullName
        );

        public string GetFileText(
            string fileFullName
        ) => File.ReadAllText(
            fileFullName
        );

        public void WriteAllText(
            string fileFullName,
            string text
        ) => File.WriteAllText(
            fileFullName,
            text
        );

        public byte[] GetFileTextAsBytes(
            string fileFullName
        ) => File.ReadAllBytes(
            fileFullName
        );

        public void WriteAllBytes(
            string fileFullName,
            byte[] bytes
        ) => File.WriteAllBytes(
            fileFullName,
            bytes
        );
    }
}
