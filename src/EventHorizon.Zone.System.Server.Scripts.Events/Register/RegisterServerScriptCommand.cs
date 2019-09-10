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

        public RegisterServerScriptCommand(
            string fileName,
            string path,
            string scriptString
        )
        {
            this.FileName = fileName;
            this.Path = path;
            this.ScriptString = scriptString;
            this.ReferenceAssemblies = Enumerable.Empty<Assembly>();
            this.Imports = Enumerable.Empty<string>();
        }

        public RegisterServerScriptCommand(
            string fileName,
            string path,
            string scriptString,
            IEnumerable<Assembly> ReferenceAssemblies,
            IEnumerable<string> Imports
        ) : this(
            fileName,
            path,
            scriptString
        )
        {
            this.ReferenceAssemblies = ReferenceAssemblies;
            this.Imports = Imports;
        }

    }
}