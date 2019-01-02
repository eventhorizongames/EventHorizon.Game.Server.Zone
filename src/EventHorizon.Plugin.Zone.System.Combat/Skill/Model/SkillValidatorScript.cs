using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Services;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;

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
                    .WithReferences(
                        typeof(SkillValidatorScript).Assembly,
                        typeof(CSharpArgumentInfo).Assembly
                    )
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "System.Numerics",
                        "EventHorizon.Game.Server.Zone.External.Extensions",
                        "EventHorizon.Game.Server.Zone.Model.Entity",
                        "EventHorizon.Game.Server.Zone.Events.Entity.Movement",
                        // TODO: Move all subnamespace Combat Events into root Events namespace
                        "EventHorizon.Plugin.Zone.System.Combat.Events.Life",
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.Model",
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction"
                    );

                using (var file = File.OpenText(this.GetFileName(scriptPath)))
                {
                    _runner = CSharpScript
                        .Create<SkillValidatorResponse>(
                            file.ReadToEnd(),
                            scriptOptions,
                            typeof(SkillValidatorScriptData))
                        .CreateDelegate();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {Id}",
                    ex
                );
            }
        }
        private string GetFileName(string scriptPath)
        {
            return Path.Combine(
                scriptPath,
                ScriptFile
            );
        }
        public async Task<SkillValidatorResponse> Run(
            IScriptServices scriptServices,
            IObjectEntity caster,
            IObjectEntity target,
            SkillInstance skill,
            IDictionary<string, object> data)
        {
            try
            {
                var response = await _runner(
                    new SkillValidatorScriptData
                    {
                        Services = scriptServices,
                        Caster = caster,
                        Target = target,
                        Skill = skill,
                        Data = data
                    });
                response.Validator = this.Id;
                return response;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(
                    $"Exception with {Id}",
                    ex
                );
            }
        }

        public class SkillValidatorScriptData
        {
            public IScriptServices Services { get; set; }
            public IObjectEntity Caster { get; set; }
            public IObjectEntity Target { get; set; }
            public SkillInstance Skill { get; set; }
            public IDictionary<string, object> Data { get; set; }
        }
    }
}