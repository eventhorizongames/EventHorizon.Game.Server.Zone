namespace EventHorizon.Zone.System.Server.Scripts.Model.Details
{
    using global::System.Collections.Generic;

    public struct ServerScriptDetails
    {
        public string Id { get; }
        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }
        public IEnumerable<string> ReferenceAssemblies { get; }
        public IEnumerable<string> TagList { get; }

        public ServerScriptDetails(
            string id,
            string fileName,
            string path,
            string scriptString,
            IEnumerable<string> referenceAssemblies,
            IEnumerable<string> tagList
        )
        {
            Id = id;
            FileName = fileName;
            Path = path;
            ScriptString = scriptString;
            ReferenceAssemblies = referenceAssemblies;
            TagList = tagList;
        }
    }
}