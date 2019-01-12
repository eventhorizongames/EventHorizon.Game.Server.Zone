using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.Agent.Ai.Script;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Model
{
    public struct AgentRoutineScript
    {
        private readonly static IDictionary<string, object> EMPTY_DATA = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

        public string RoutineName { get; set; }
        public string FileName { get; set; }
        private ScriptRunner<AgentRoutineScriptResponse> _runner;

        public bool IsFound()
        {
            return this.RoutineName != null;
        }

        public AgentRoutineScript CreateScript(string scriptPath)
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(
                        typeof(AgentRoutineScript).Assembly,
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
                        "EventHorizon.Game.Server.Zone.Events.Path",
                        "EventHorizon.Game.Server.Zone.Agent.Events",
                        "EventHorizon.Game.Server.Zone.Agent.Model",
                        "EventHorizon.Game.Server.Zone.Agent.Move",

                        // TODO: Move all subnamespace Agent AI Events into root Events namespace
                        "EventHorizon.Game.Server.Zone.Agent.Ai.Model",
                        "EventHorizon.Game.Server.Zone.Agent.Ai.Move"
                    );

                using (var file = File.OpenText(this.GetFileName(scriptPath)))
                {
                    _runner = CSharpScript
                        .Create<AgentRoutineScriptResponse>(
                            file.ReadToEnd(),
                            scriptOptions,
                            typeof(AgentRoutineScriptData))
                        .CreateDelegate();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {RoutineName}",
                    ex
                );
            }
            return this;
        }
        private string GetFileName(string scriptPath)
        {
            return Path.Combine(
                scriptPath,
                FileName
            );
        }
        public async Task<AgentRoutineScriptResponse> Run(
            IScriptServices services,
            IObjectEntity agent,
            IDictionary<string, object> data)
        {
            try
            {
                return await _runner(
                    new AgentRoutineScriptData
                    {
                        Services = services,
                        Agent = agent,
                        Data = data ?? EMPTY_DATA,
                    });
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(
                    $"Exception with {RoutineName}",
                    ex
                );
            }
        }

        public class AgentRoutineScriptData
        {
            public IScriptServices Services { get; set; }
            public IObjectEntity Agent { get; set; }
            public IDictionary<string, object> Data { get; set; }
        }
    }
}