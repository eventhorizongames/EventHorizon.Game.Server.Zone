namespace EventHorizon.Zone.System.Server.Scripts.Events.Register
{
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Reflection;
    using MediatR;

    public struct RegisterServerScriptCommand
        : IRequest
    {

        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }
        public IEnumerable<Assembly> ReferenceAssemblies { get; }
        public IEnumerable<string> TagList { get; }

        public RegisterServerScriptCommand(
            string fileName,
            string path,
            string scriptString
        )
        {
            FileName = fileName;
            Path = path;
            ScriptString = scriptString;
            ReferenceAssemblies = Enumerable.Empty<Assembly>();
            TagList = Enumerable.Empty<string>();
        }

        public RegisterServerScriptCommand(
            string fileName,
            string path,
            string scriptString,
            IEnumerable<Assembly> referenceAssemblies
        ) : this(
            fileName,
            path,
            scriptString
        )
        {
            ReferenceAssemblies = referenceAssemblies;
        }

        public RegisterServerScriptCommand(
            string fileName,
            string path,
            string scriptString,
            IEnumerable<Assembly> referenceAssemblies,
            IEnumerable<string> tagList
        ) : this(
            fileName,
            path,
            scriptString
        )
        {
            ReferenceAssemblies = referenceAssemblies;
            TagList = tagList;
        }
    }
}
