namespace EventHorizon.Zone.System.Server.Scripts.System
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Reflection;
    using global::System.Security.Cryptography;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CSharp.RuntimeBinder;

    public class SystemServerScript : ServerScript
    {
        private static HashAlgorithm HashAlgorithm => MD5.Create();

        public static ServerScript Create(
            string fileName,
            string path,
            string scriptAsString,
            IEnumerable<Assembly> referenceAssemblies
        )
        {
            return new SystemServerScript(
                new ScriptDetails
                {
                    FileName = fileName,
                    Path = path,
                    ScriptAsString = scriptAsString,
                    ReferenceAssemblies = referenceAssemblies,
                }
            );
        }

        private struct ScriptDetails
        {
            public string FileName { get; set; }
            public string Path { get; set; }
            public string ScriptAsString { get; set; }
            public IEnumerable<Assembly> ReferenceAssemblies { get; set; }
        }

        public string Id { get; }
        public string Hash { get; }

        private readonly ScriptRunner<ServerScriptResponse> _runner;

        private bool IsFound()
        {
            return Id != null;
        }

        private SystemServerScript(
            ScriptDetails details
        )
        {
            Id = GenerateId(
                details.Path,
                details.FileName
            );
            Hash = GenerateHash(
                details.ScriptAsString
            );

            _runner = CreateRunner(
                details
            );
        }

        public Task<ServerScriptResponse> Run(
            ServerScriptServices services,
            IDictionary<string, object> data
        ) => _runner(
            new SystemServerScriptData(
                services,
                data
            )
        );

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

        public static string GenerateId(
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
                return id[1..];
            }
            return id;
        }

        public static string GenerateHash(
            string content
        )
        {
            return Convert.ToBase64String(
                HashAlgorithm.ComputeHash(
                    content.ToBytes()
                )
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
                    ).AddReferences(
                        details.ReferenceAssemblies
                    ).AddImports(
                        "System",
                        "System.Collections.Generic",
                        "System.Numerics"
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
            finally
            {
                GC.Collect();
            }
        }
    }
}