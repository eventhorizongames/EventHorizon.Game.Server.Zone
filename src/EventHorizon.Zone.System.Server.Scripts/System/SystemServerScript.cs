using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using IOPath = System.IO.Path;
using System.Reflection;
using System.Dynamic;

namespace EventHorizon.Zone.System.Server.Scripts.System
{
    public struct SystemServerScript : ServerScript
    {
        public static ServerScript Create(
            string fileName,
            string path,
            string scriptAsString,
            IEnumerable<Assembly> referenceAssemblies,
            IEnumerable<string> imports
        )
        {
            return new SystemServerScript(
                new ScriptDetails
                {
                    FileName = fileName,
                    Path = path,
                    ScriptAsString = scriptAsString,
                    ReferenceAssemblies = referenceAssemblies,
                    Imports = imports
                }
            );
        }
        private struct ScriptDetails
        {
            public string FileName { get; set; }
            public string Path { get; set; }
            public string ScriptAsString { get; set; }
            public IEnumerable<Assembly> ReferenceAssemblies { get; set; }
            public IEnumerable<string> Imports { get; set; }
        }

        public string Id { get; }
        public string ScriptAsString { get; }
        private ScriptRunner<ServerScriptResponse> _runner;

        private bool IsFound()
        {
            return Id != null;
        }

        private SystemServerScript(
            ScriptDetails details
        )
        {
            this.Id = GenerateId(
                details.Path,
                details.FileName
            );
            this.ScriptAsString = details.ScriptAsString;
            this._runner = CreateRunner(
                details
            );
        }
        public async Task<ServerScriptResponse> Run(
            ServerScriptServices services,
            IDictionary<string, object> data
        )
        {
            if (!IsFound())
            {
                throw new ServerScriptNotFound();
            }
            return await _runner(
                new SystemServerScriptData(
                    services,
                    data
                )
            );
        }

        public class SystemServerScriptData
        {
            public ServerScriptServices Services { get; }
            public dynamic Data { get; }
            public SystemServerScriptData(
                ServerScriptServices services,
                IDictionary<string, object> data
            )
            {
                Services = services;
                Data = new DynamicData(
                    data
                );
            }
        }

        public class DynamicData : DynamicObject
        {
            readonly IDictionary<string, object> _properties;

            public DynamicData(
                IDictionary<string, object> properties
            )
            {
                _properties = properties;
            }
            public override bool TryGetMember(
                GetMemberBinder binder,
                out object result
            )
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    result = _properties[binder.Name];
                    return true;
                }
                else
                {
                    result = "Invalid Property!";
                    return false;
                }
            }

            public override bool TrySetMember(
                SetMemberBinder binder,
                object value
            )
            {
                _properties[binder.Name] = value;
                return true;
            }

            public override bool TryInvokeMember(
                InvokeMemberBinder binder,
                object[] args,
                out object result
            )
            {
                dynamic method = _properties[binder.Name];
                result = method(args[0].ToString(), args[1].ToString());
                return true;
            }
        }

        private static string GenerateId(
            string path,
            string fileName
        )
        {
            return string.Join(
                "_",
                string.Join(
                    "_",
                    path.Split(
                        IOPath.DirectorySeparatorChar
                    )
                ),
                fileName
            );
        }

        private static ScriptRunner<ServerScriptResponse> CreateRunner(
            ScriptDetails details
        )
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .AddReferences(
                        typeof(SystemServerScript).Assembly,
                        typeof(CSharpArgumentInfo).Assembly
                    // TODO: Load extra Assemblies from Details
                    ).AddReferences(
                        details.ReferenceAssemblies
                    ).AddImports(
                        "System",
                        "System.Collections.Generic",
                        "System.Numerics"
                    // TODO: Load extra imports from Details
                    // "EventHorizon.Zone.Core.Model.Extensions",
                    // "EventHorizon.Zone.Core.Model.Entity",
                    // "EventHorizon.Zone.Core.Events.Entity.Movement",
                    // "EventHorizon.Zone.Core.Events.Map",
                    // "EventHorizon.Zone.Core.Events.Path"
                    ).AddImports(
                        details.Imports
                    );

                return CSharpScript
                    .Create<ServerScriptResponse>(
                        details.ScriptAsString,
                        scriptOptions,
                        typeof(SystemServerScriptData))
                    .CreateDelegate();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with  {details.Path} | {details.FileName}",
                    ex
                );
            }
        }
    }
}