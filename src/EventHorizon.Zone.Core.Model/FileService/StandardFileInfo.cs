namespace EventHorizon.Zone.Core.Model.FileService
{
    public struct StandardFileInfo
    {
        public string Name { get; }
        public string DirectoryName { get; }
        public string FullName { get; }
        public string Extension { get; }

        public StandardFileInfo(
            string name,
            string directoryName,
            string fullName,
            string extension
        )
        {
            Name = name;
            DirectoryName = directoryName;
            FullName = fullName;
            Extension = extension;
        }
    }
}
