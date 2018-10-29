using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Json.Impl;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Models;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Runner;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using Xunit;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Tests.Combat.Skill.Runner
{
    public class SkillRunnerTests
    {
        string assetsPath = @"C:\Repos\EventHorizon.Game\EventHorizon.Game.Server.Zone\src\EventHorizon.Plugin.Zone.System.Combat\Assets";

        [Fact]
        public async Task Test_ExpectedFireBallSkill()
        {
            //Given
            var jsonFileLoader = new JsonFileLoader();
            var combatSkillsFile = await jsonFileLoader.GetFile<SkillListFile>(
                IOPath.Combine(
                    assetsPath,
                    "Combat.Skills.json"
                )
            );
            var skillSystemCombatFile = await jsonFileLoader.GetFile<SkillSystemFile>(
                IOPath.Combine(
                    assetsPath,
                    "Skill.System.Combat.json"
                )
            );

            var skillEffectScriptRepository = new SkillEffectScriptRepository();

            foreach (var skillEffect in skillSystemCombatFile.EffectList)
            {
                skillEffect.CreateScript(IOPath.Combine(
                    assetsPath,
                    "Scripts",
                    "Effects"
                ));
                skillEffectScriptRepository.Add(
                    skillEffect
                );
            }

            var casterId = 1000;
            var caster = new TestObjectEntity
            {
                Id = casterId,
                RawData = new Dictionary<string, object>()
            };
            caster.SetProperty<LifeState>("LifeState", new LifeState
            {
                ActionPoints = 100,
                    HealthPoints = 100
            });
            caster.SetProperty<LevelState>(LevelState.PROPERTY_NAME, new LevelState
            {
                ActionPointsLevel = 100,
                    HealthPointsLevel = 100,
            });

            var targetId = 2000;
            var target = new TestObjectEntity
            {
                Id = targetId
            };
            target.SetProperty<LifeState>("LifeState", new LifeState
            {
                ActionPoints = 100,
                    HealthPoints = 100
            });
            target.SetProperty<LevelState>(LevelState.PROPERTY_NAME, new LevelState
            {
                ActionPointsLevel = 100,
                    HealthPointsLevel = 100,
            });

            //When
            var skillRunner = new SkillRunner(
                skillEffectScriptRepository
            );

            var actual = await skillRunner.Run(
                combatSkillsFile.SkillList[0],
                caster,
                target
            );

            //Then

            Assert.NotEmpty(actual);
        }
    }
}