using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Events.Register
{
    public struct ServerScriptRegisteredEvent : INotification
    {
        public string Id { get; }
        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }
        public IEnumerable<Assembly> ReferenceAssemblies { get; }
        public IEnumerable<string> Imports { get; }
        public IEnumerable<string> TagList { get; }

        public ServerScriptRegisteredEvent(
            string id,
            string fileName,
            string path,
            string scriptString,
            IEnumerable<Assembly> referenceAssemblies,
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