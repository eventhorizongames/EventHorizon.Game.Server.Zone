using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Script
{
    public struct BehaviorScript
    {
        public string Id { get; }
        private Script<BehaviorScriptResponse> _runner;

        public BehaviorScript(
            string id,
            Script<BehaviorScriptResponse> runner
        )
        {
            this.Id = id;
            _runner = runner;
        }

        public static BehaviorScript CreateScript(
            string id,
            string scriptContent
        )
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(
                        typeof(BehaviorScript).Assembly,
                        typeof(SystemAgentPluginAiExtensions).Assembly,
                        typeof(SystemAgentPluginMoveExtensions).Assembly,
                        typeof(CSharpArgumentInfo).Assembly
                    )
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "EventHorizon.Zone.System.Agent.Plugin.Behavior.Script",
                        "EventHorizon.Zone.System.Agent.Plugin.Behavior.Model",

                        "EventHorizon.Zone.Core.Model.Entity",
                        "EventHorizon.Zone.Core.Events.Entity.Movement"
                    );

                var runner = CSharpScript
                    .Create<BehaviorScriptResponse>(
                        scriptContent,
                        scriptOptions,
                        typeof(BehaviorScriptData)
                    );
                // This will try and complie the script.
                // If it fails the Catch will capture this and the passed on to the loader.
                runner.CreateDelegate();
                runner.Compile();
                return new BehaviorScript(
                    id,
                    runner
                );
            }//"(16,18): error CS0656: Missing compiler required member 'Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create'"
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {id}",
                    ex
                );
            }
        }
        public async Task<BehaviorScriptResponse> Run(
            ServerScriptServices services,
            IObjectEntity actor
        )
        {
            try
            {
                return (await _runner.RunAsync(
                    new BehaviorScriptData
                    {
                        Services = services,
                        Actor = actor
                    })).ReturnValue;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(
                    $"Exception with {Id}",
                    ex
                );
            }
        }

        public class BehaviorScriptData
        {
            public ServerScriptServices Services { get; set; }
            public IObjectEntity Actor { get; set; }
        }
    }
}