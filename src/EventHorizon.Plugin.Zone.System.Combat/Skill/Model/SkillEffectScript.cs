using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly static IDictionary<string, object> EMPTY_STATE = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
        public string Id { get; set; }
        public string ScriptFile { get; set; }
        private ScriptRunner<SkillEffectScriptResponse> _runner;

        public void CreateScript(string scriptPath)
        {
            try
            {
                var scriptOptions = ScriptOptions
                    .Default
                    .WithReferences(typeof(ClientSkillActionEvent).Assembly)
                    .WithImports(
                        "System",
                        "System.Collections.Generic",
                        "EventHorizon.Game.Server.Zone.External.Extensions",
                        "EventHorizon.Game.Server.Zone.Model.Entity",
                        "EventHorizon.Game.Server.Zone.Events.Entity.Movement",
                        // TODO: Move all subnamespace Combat Events into root Events namespace
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.Model",
                        "EventHorizon.Plugin.Zone.System.Combat.Client",
                        "EventHorizon.Plugin.Zone.System.Combat.Events.Life",
                        "EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction"
                    );

                using (var file = File.OpenText(this.GetFileName(scriptPath)))
                {
                    _runner = CSharpScript
                        .Create<SkillEffectScriptResponse>(
                            file.ReadToEnd(),
                            scriptOptions,
                            typeof(SkillEffectScriptData))
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
        public async Task<SkillEffectScriptResponse> Run(
            IMediator mediator,
            IObjectEntity caster,
            IObjectEntity target,
            IDictionary<string, object> data,
            IDictionary<string, object> priorState)
        {
            try
            {
                var services = new SkillEffectScriptServicesData
                {
                    Mediator = mediator
                };
                return await _runner(
                    new SkillEffectScriptData
                    {
                        Services = services,
                        Caster = caster,
                        Target = target,
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
            public SkillEffectScriptServicesData Services { get; set; }
            public IObjectEntity Caster { get; set; }
            public IObjectEntity Target { get; set; }
            public IDictionary<string, object> Data { get; set; }
            public IDictionary<string, object> PriorState { get; set; }
        }
        public struct SkillEffectScriptServicesData
        {
            public IMediator Mediator { get; set; }
        }
    }
}