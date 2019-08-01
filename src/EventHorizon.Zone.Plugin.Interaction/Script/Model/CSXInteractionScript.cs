using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Player;
using EventHorizon.Zone.Plugin.Interaction.Script.Api;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using MSScript = Microsoft.CodeAnalysis.Scripting.Script;

namespace EventHorizon.Zone.Plugin.Interaction.Script.Model
{
    public struct CSXInteractionScript : InteractionScript
    {
        public string Id { get; }
        private MSScript _runner;

        public CSXInteractionScript(
            string id,
            MSScript runner
        )
        {
            this.Id = id;
            _runner = runner;
        }

        public static InteractionScript Create(
            string id,
            string scriptContent
        )
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(
                        typeof(CSXInteractionScript).Assembly,
                        typeof(CSharpArgumentInfo).Assembly
                    )
                    .WithImports(
                        "System"
                    );

                var runner = CSharpScript
                    .Create(
                        scriptContent,
                        scriptOptions,
                        typeof(InteractionScriptData)
                    );
                runner.Compile();
                return new CSXInteractionScript(
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
        public async Task<bool> Run(
            PlayerEntity player,
            IObjectEntity target,
            IDictionary<string, object> data
            // IServerScriptServices services,
        )
        {
            try
            {
                await _runner.RunAsync(
                    new InteractionScriptData
                    {
                        Player = player,
                        Target = target,
                        Data = data
                        // Services = services,
                    }
                );
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(
                    $"Exception with {Id}",
                    ex
                );
            }
        }

        public class InteractionScriptData
        {
            public PlayerEntity Player { get; set; }
            public IObjectEntity Target { get; set; }
            public IDictionary<string, object> Data { get; set; }
        }
    }
}