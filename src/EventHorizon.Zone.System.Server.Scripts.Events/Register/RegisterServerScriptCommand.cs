using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Events.Register
{
    public struct RegisterServerScriptCommand : IRequest
    {

        public string FileName { get; }
        public string Path { get; }
        public string ScriptString { get; }
        public IEnumerable<Assembly> ReferenceAssemblies { get; }
        public IEnumerable<string> Imports { get; }
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
            Imports = Enumerable.Empty<string>();
            TagList = Enumerable.Empty<string>();
        }

        public RegisterServerScriptCommand(
            string fileName,
            string path,
            string scriptString,
            IEnumerable<Assembly> referenceAssemblies,
            IEnumerable<string> imports
        ) : this(
            fileName,
            path,
            scriptString
        )
        {
            ReferenceAssemblies = referenceAssemblies;
            Imports = imports;
        }

        public RegisterServerScriptCommand(
            string fileName,
            string path,
            string scriptString,
            IEnumerable<Assembly> referenceAssemblies,
            IEnumerable<string> imports,
            IEnumerable<string> tagList
        ) : this(
            fileName,
            path,
            scriptString
        )
        {
            ReferenceAssemblies = referenceAssemblies;
            Imports = imports;
            TagList = tagList;
        }

    }
}