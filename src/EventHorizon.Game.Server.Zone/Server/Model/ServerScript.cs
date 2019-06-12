using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Server.Api;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Server.Model
{
    public struct ServerScript
    {
        public static ServerScript Create(
            ServerScriptDetails details
        )
        {
            return new ServerScript(
                details
            );
        }
        public struct ServerScriptDetails
        {
            public string FileName { get; set; }
            public string Path { get; set; }
            public string ScriptString { get; set; }
        }

        public string Id { get; set; }
        public string ScriptString { get; set; }
        private ScriptRunner<object> _runner;

        private bool IsFound()
        {
            return Id != null;
        }

        private ServerScript(
            ServerScriptDetails details
        )
        {
            this.Id = details.FileName;
            this.ScriptString = details.ScriptString;
            this._runner = null;

            this.Id = this.GenerateId(
                details.Path,
                details.FileName
            );
            this._runner = this.CreateRunner(
                details
            );
        }

        private string GenerateId(
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

        private ScriptRunner<object> CreateRunner(
            ServerScriptDetails details
        )
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(
                        typeof(ServerScript).Assembly,
                        typeof(CSharpArgumentInfo).Assembly
                    )
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "System.Numerics",
                        "EventHorizon.Game.Server.Zone.External.Extensions",
                        "EventHorizon.Game.Server.Zone.Model.Entity",
                        "EventHorizon.Game.Server.Zone.Events.Entity.Movement",
                        "EventHorizon.Game.Server.Zone.Events.Map",
                        "EventHorizon.Game.Server.Zone.Events.Path"
                    );

                return CSharpScript
                    .Create<object>(
                        details.ScriptString,
                        scriptOptions,
                        typeof(ServerScriptData))
                    .CreateDelegate();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {details.FileName}",
                    ex
                );
            }
        }
        public async Task<T> Run<T>(
            IServerScriptServices services,
            IDictionary<string, object> data
        )
        {
            try
            {
                if (!IsFound())
                {
                    return default(T);
                }
                return (T)await _runner(
                    new ServerScriptData
                    {
                        Services = services,
                        Data = data
                    });
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(
                    $"Exception with {Id}",
                    ex
                );
            }
        }

        public class ServerScriptData
        {
            public IServerScriptServices Services { get; set; }
            public IDictionary<string, object> Data { get; set; }
        }
    }
}