namespace EventHorizon.Zone.System.Server.Scripts.Events.Register
{
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Reflection;
    using MediatR;

    public struct ServerScriptRegisteredEvent
        : INotification
    {
        public string Id { get; }
        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }
        public IEnumerable<Assembly> ReferenceAssemblies { get; }
        public IEnumerable<string> TagList { get; }

        public ServerScriptRegisteredEvent(
            string id,
            string fileName,
            string path,
            string scriptString,
            IEnumerable<Assembly> referenceAssemblies,
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