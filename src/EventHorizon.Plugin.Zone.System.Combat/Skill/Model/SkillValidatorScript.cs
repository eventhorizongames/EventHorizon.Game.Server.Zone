using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Plugin.Zone.System.Combat.Script;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using System.Numerics;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillValidatorScript
    {
        public string Id { get; set; }
        private ScriptRunner<SkillValidatorResponse> _runner;
        private SkillValidatorScript(
            string id,
            ScriptRunner<SkillValidatorResponse> runner
        )
        {
            this.Id = id;
            _runner = runner;
        }

        public static SkillValidatorScript CreateScript(
            string id,
            string scriptContent
        )
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
                        "EventHorizon.Zone.Core.Model.Entity",
                        "EventHorizon.Game.Server.Zone.Events.Entity.Movement",
                        // TODO: Move these to the Script using statements
                        "EventHorizon.Plugin.Zone.System.Combat.Events.Life",
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.Model",
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction"
                    );

                var runner = CSharpScript
                    .Create<SkillValidatorResponse>(
                        scriptContent,
                        scriptOptions,
                        typeof(SkillValidatorScriptData))
                    .CreateDelegate();
                return new SkillValidatorScript(
                    id,
                    runner
                );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {id}",
                    ex
                );
            }
        }
        public async Task<SkillValidatorResponse> Run(
            IScriptServices scriptServices,
            IObjectEntity caster,
            IObjectEntity target,
            SkillInstance skill,
            Vector3 targetPosition,
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
                        TargetPosition = targetPosition,
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
            public Vector3 TargetPosition { get; set; }
            public IDictionary<string, object> Data { get; set; }
        }
    }
}