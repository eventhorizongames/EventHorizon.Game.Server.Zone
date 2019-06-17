using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.Agent.Ai.Script;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;

namespace EventHorizon.Zone.System.Agent.Behavior.Script
{
    public struct BehaviorScript
    {
        public string Id { get; }
        private ScriptRunner<BehaviorScriptResponse> _runner;

        private BehaviorScript(
            string id,
            ScriptRunner<BehaviorScriptResponse> runner
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
                        typeof(CSharpArgumentInfo).Assembly
                    )
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "EventHorizon.Game.Server.Zone.External.Extensions",
                        "EventHorizon.Game.Server.Zone.Model.Entity",
                        "EventHorizon.Game.Server.Zone.Events.Entity.Movement",
                        "EventHorizon.Game.Server.Zone.Agent.Ai.Move"
                    );

                var runner = CSharpScript
                    .Create<BehaviorScriptResponse>(
                        scriptContent,
                        scriptOptions,
                        typeof(BehaviorScriptData))
                    .CreateDelegate();
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
            IScriptServices services,
            IObjectEntity actor
        )
        {
            try
            {
                return await _runner(
                    new BehaviorScriptData
                    {
                        Services = services,
                        Actor = actor
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

        public class BehaviorScriptData
        {
            public IScriptServices Services { get; set; }
            public IObjectEntity Actor { get; set; }
        }
    }
}