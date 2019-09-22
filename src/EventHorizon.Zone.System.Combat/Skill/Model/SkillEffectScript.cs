using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using EventHorizon.Zone.System.Combat.Script;
using System.Numerics;
using EventHorizon.Game.Server.Zone;

namespace EventHorizon.Zone.System.Combat.Skill.Model
{
    public struct SkillEffectScript
    {
        private readonly static IDictionary<string, object> EMPTY_STATE = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
        public string Id { get; }
        private ScriptRunner<SkillEffectScriptResponse> _runner;

        private SkillEffectScript(
            string id,
            ScriptRunner<SkillEffectScriptResponse> runner
        )
        {
            this.Id = id;
            _runner = runner;
        }

        public static SkillEffectScript CreateScript(
            string id,
            string scriptContent
        )
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(
                        typeof(SkillEffectScript).Assembly,
                        typeof(CSharpArgumentInfo).Assembly,
                        // This is necessary to resolve the supported imports
                        typeof(SystemAgentExtensions).Assembly,
                        typeof(SystemAgentAiExtensions).Assembly
                    )
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "EventHorizon.Zone.Core.Model.Extensions",
                        "EventHorizon.Zone.Core.Model.Entity",
                        "EventHorizon.Zone.Core.Events.Entity.Movement",
                        "EventHorizon.Game.Server.Zone.Agent.Ai.Move",

                        // TODO: Move these to the Script using statements
                        "EventHorizon.Zone.System.Combat.Skill.Model",
                        "EventHorizon.Zone.System.Combat.Client",
                        "EventHorizon.Zone.System.Combat.Events.Life",
                        "EventHorizon.Zone.System.Combat.Skill.ClientAction"
                    );

                var runner = CSharpScript
                    .Create<SkillEffectScriptResponse>(
                        scriptContent,
                        scriptOptions,
                        typeof(SkillEffectScriptData))
                    .CreateDelegate();
                return new SkillEffectScript(
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
        public async Task<SkillEffectScriptResponse> Run(
            IScriptServices services,
            IObjectEntity caster,
            IObjectEntity target,
            SkillInstance skill,
            Vector3 targetPosition,
            IDictionary<string, object> data,
            IDictionary<string, object> priorState)
        {
            try
            {
                return await _runner(
                    new SkillEffectScriptData
                    {
                        Services = services,
                        Caster = caster,
                        Target = target,
                        Skill = skill,
                        TargetPosition = targetPosition,
                        Data = data,
                        PriorState = priorState ?? EMPTY_STATE
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
            public IScriptServices Services { get; set; }
            public IObjectEntity Caster { get; set; }
            public IObjectEntity Target { get; set; }
            public SkillInstance Skill { get; set; }
            public Vector3 TargetPosition { get; set; }
            public IDictionary<string, object> Data { get; set; }
            public IDictionary<string, object> PriorState { get; set; }
        }
    }
}