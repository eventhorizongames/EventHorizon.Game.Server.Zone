using EventHorizon.Game.Server.Zone.Tests.TestUtil.Models;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Validators;
using Xunit;
using EventHorizon.Game.Server.Zone.Model.Entity;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Combat.Skill.Validators
{
    public class CasterHasCostApValidatorTests
    {
        [Fact]
        public void TestDynamicProperties()
        {
            //Given
            var caster = new TestObjectEntity();
            caster.RawData = new Dictionary<string, object>();
            caster.SetProperty<LifeState>("LifeState", new LifeState
            {
                ActionPoints = 100
            });
            var data = new Dictionary<string, object>();
            data["amount"] = 100;

            //When
            var validator = new CasterHasCostApValidator();
            var actual = validator.check(
                caster,
                null,
                data
            );

            //Then
        }
    }
}