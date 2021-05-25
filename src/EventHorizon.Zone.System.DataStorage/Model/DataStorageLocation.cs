namespace EventHorizon.Zone.System.DataStorage.Model
{
    using global::System.IO;

    public static class DataStorageLocation
    {
        private const string PATH = "DataStorage";
        private const string FILE_NAME = "DataStore.json";

        public static (string DirectoryFullName, string FileFullName) GenerateDataStorageLocation(
            string rootPath
        )
        {
            var directory = Path.Combine(
                rootPath,
                PATH
            );
            var fileFullName = Path.Combine(
                directory,
                FILE_NAME
            );

            return (directory, fileFullName);
        }
    }
}
