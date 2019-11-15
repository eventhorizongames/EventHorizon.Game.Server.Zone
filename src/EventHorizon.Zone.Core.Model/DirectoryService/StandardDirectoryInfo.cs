namespace EventHorizon.Zone.Core.Model.DirectoryService
{
    public struct StandardDirectoryInfo
    {
        public string Name { get; }
        public string FullName { get; }
        public string ParentFullName { get; }

        public StandardDirectoryInfo(
            string name,
            string fullName,
            string parentFullName
        )
        {
            Name = name;
            FullName = fullName;
            ParentFullName = parentFullName;
        }
    }
}