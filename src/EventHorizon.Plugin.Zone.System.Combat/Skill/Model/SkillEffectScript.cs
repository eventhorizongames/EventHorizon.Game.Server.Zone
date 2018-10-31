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
        public string ScriptFile { get; set; }
        public string ClientScriptFile { get; set; }
        private ScriptRunner<List<ClientSkillActionEvent>> _runner;

        public void CreateScript(string scriptFolder)
        {
            try
            {

                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(typeof(ClientSkillActionEvent).Assembly)
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction",
                        "EventHorizon.Game.Server.Zone.Model.Entity"
                    );

                _runner = CSharpScript
                    .Create<List<ClientSkillActionEvent>>(
                        File.OpenText(
                            Path.Combine(
                                scriptFolder,
                                ScriptFile
                            )
                        ).ReadToEnd(),
                        scriptOptions,
                        typeof(SkillEffectScriptData))
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