using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillEffectScript
    {
        public string Id { get; set; }
        public string ServerScriptFile { get; set; }
        public string ClientScriptFile { get; set; }
        private ScriptRunner<List<ClientSkillActionEvent>> _runner;

        public void CreateScript(string scriptFolder)
        {
            try
            {

                var scriptOptions = ScriptOptions
                    .Default
                    // .WithReferences(typeof(IObjectEntity).Assembly)
                    // .WithImports("System.Collections.Generic")
                    // .WithReferences(typeof(IObjectEntity).Assembly)
                    // .WithImports("EventHorizon.Game.Server.Zone.Model.Entity")
                    // TODO: Make this a Object data based Client Action event.
                    .WithReferences(typeof(ClientSkillActionEvent).Assembly)
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction",
                        "EventHorizon.Game.Server.Zone.Model.Entity"
                    );

                _runner = CSharpScript
                    .Create<List<ClientSkillActionEvent>>(
                        "// Skill Effect Script Generated",
                        scriptOptions,
                        globalsType : typeof(SkillEffectScriptData))
                    // .ContinueWith<List<ClientSkillActionEvent>>("using System;")
                    // .ContinueWith<List<ClientSkillActionEvent>>("using System.Collections.Generic;")
                    .ContinueWith<List<ClientSkillActionEvent>>(File.OpenText(
                        Path.Combine(
                            scriptFolder,
                            ServerScriptFile
                        )
                    ).ReadToEnd())
                    .CreateDelegate();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {Id}",
                    ex
                );
            }
        }
        public async Task<List<ClientSkillActionEvent>> Run(
            IObjectEntity caster,
            IObjectEntity target,
            IDictionary<string, object> data)
        {
            try
            {
                return await _runner(
                    new SkillEffectScriptData
                    {
                        Caster = caster,
                            Target = target,
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

        public class SkillEffectScriptData
        {
            public IObjectEntity Caster { get; set; }
            public IObjectEntity Target { get; set; }
            public IDictionary<string, object> Data { get; set; }
        }
    }
}