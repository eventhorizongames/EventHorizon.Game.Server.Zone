using System.Collections.Generic;

namespace EventHorizon.Zone.System.Server.Scripts.Model.Details
{
    public struct ServerScriptDetails
    {
        public string Id { get; }
        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }
        public IEnumerable<string> ReferenceAssemblies { get; }
        public IEnumerable<string> Imports { get; }
        public IEnumerable<string> TagList { get; }

        public ServerScriptDetails(
            string id,
            string fileName,
            string path,
            string scriptString,
            IEnumerable<string> referenceAssemblies,
            IEnumerable<string> imports,
            IEnumerable<string> tagList
        )
        {
            Id = id;
            FileName = fileName;
            Path = path;
            ScriptString = scriptString;
            ReferenceAssemblies = referenceAssemblies;
            Imports = imports;
            TagList = tagList;
        }
    }
}