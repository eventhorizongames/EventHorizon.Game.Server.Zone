using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using System.Reflection;
using System.IO;

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
            public DynamicData Data { get; }
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

        public struct DynamicData
        {
            private IDictionary<string, object> _data;

            public DynamicData(
                IDictionary<string, object> data
            )
            {
                _data = data;
            }

            public T Get<T>(
                string name
            )
            {
                if (!_data.ContainsKey(
                    name
                ))
                {
                    return default(T);
                }
                return (T)_data[name];
            }
        }

        private static string GenerateId(
            string path,
            string fileName
        )
        {
            var id = string.Join(
                "_",
                string.Join(
                    "_",
                    path.Split(
                        new char[] { Path.DirectorySeparatorChar },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                ),
                fileName
            );
            if (id.StartsWith(
                "_"
            ))
            {
                return id.Substring(
                    1
                );
            }
            return id;
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
                    ).AddReferences(
                        details.ReferenceAssemblies
                    ).AddImports(
                        "System",
                        "System.Collections.Generic",
                        "System.Numerics"
                    ).AddImports(
                        details.Imports
                    );

                return CSharpScript
                    .Create<ServerScriptResponse>(
                        details.ScriptAsString,
                        scriptOptions,
                        typeof(SystemServerScriptData)
                    ).CreateDelegate();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {details.Path} | {details.FileName}",
                    ex
                );
            }
        }
    }
}