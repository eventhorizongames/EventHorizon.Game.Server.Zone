namespace EventHorizon.Zone.Core.Model.FileService
{
    public interface FileResolver
    {
        /// <summary>
        /// This will take in a file FullName,
        /// Try and create file,
        /// No text will be added.
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        bool CreateFile(
            string fileFullName
        );
        StandardFileInfo GetFileInfo(
            string fileFullName
        );

        /// <summary>
        /// This will take in a file FullName, 
        /// Create if does not exist,
        /// Then append text to file.
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        bool AppendTextToFile(
            string fileFullName,
            string text
        );
        void DeleteFile(
            string fileFullName
        );
        bool DoesFileExist(
            string fileFullName
        );
        string GetFileText(
            string fileFullName
        );
        void WriteAllText(
            string fileFullName,
            string text
        );
        byte[] GetFileTextAsBytes(
            string fileFullName
        );
        void WriteAllBytes(
            string fileFullName,
            byte[] bytes
        );
    }
}
