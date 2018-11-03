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
    public struct SkillValidatorScript
    {
        public string Id { get; set; }
        public string ScriptFile { get; set; }
        private ScriptRunner<SkillValidatorResponse> _runner;

        public void CreateScript(string scriptPath)
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(typeof(SkillValidatorScript).Assembly)
                    .WithImports(
                        "System",
                        "System.Collections.Generic"
                    );

                _runner = CSharpScript
                    .Create<SkillValidatorResponse>(
                        File.OpenText(
                            Path.Combine(
                                scriptPath,
                                ScriptFile
                            )
                        ).ReadToEnd(),
                        scriptOptions,
                        typeof(SkillValidatorScriptData))
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
        public async Task<SkillValidatorResponse> Run(
            IMediator mediator,
            IObjectEntity caster,
            IObjectEntity target,
            IDictionary<string, object> data)
        {
            try
            {
                var services = new SkillValidatorScriptServicesData
                {
                    Mediator = mediator
                };
                return await _runner(
                    new SkillValidatorScriptData
                    {
                        Services = services,
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

        public struct SkillValidatorScriptData
        {
            public SkillValidatorScriptServicesData Services { get; set; }
            public IObjectEntity Caster { get; set; }
            public IObjectEntity Target { get; set; }
            public IDictionary<string, object> Data { get; set; }
        }
        public struct SkillValidatorScriptServicesData
        {
            public IMediator Mediator { get; set; }
        }
    }
}