using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Models;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Validators;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Combat.Skill.Validators
{
    public class CasterHasCostApValidatorTests
    {
        [Fact]
        public void TestDynamicProperties_ShouldReturnTrueWhenCostIsLessThanContainedAmount()
        {
            //Given
            var caster = new TestObjectEntity();
            caster.RawData = new Dictionary<string, object>();
            caster.SetProperty<LifeState>("LifeState", new LifeState
            {
                ActionPoints = 100
            });
            var data = new Dictionary<string, object>();
            data["propertyName"] = "LifeState";
            data["valueProperty"] = "ActionPoints";
            data["cost"] = 10;

            //When
            var validator = new CasterHasCostApValidator();
            var actual = validator.check(
                caster,
                null,
                data
            );

            //Then
            Assert.True(actual);
        }
        [Fact]
        public void TestDynamicProperties_ShouldReturnFalseWhenCostIsLessThanContainedAmount()
        {
            //Given
            var caster = new TestObjectEntity();
            caster.RawData = new Dictionary<string, object>();
            caster.SetProperty<LifeState>("LifeState", new LifeState
            {
                ActionPoints = 10
            });
            var data = new Dictionary<string, object>();
            data["propertyName"] = "LifeState";
            data["valueProperty"] = "ActionPoints";
            data["cost"] = 100;

            //When
            var validator = new CasterHasCostApValidator();
            var actual = validator.check(
                caster,
                null,
                data
            );

            //Then
            Assert.False(actual);
        }
        [Fact]
        public void TestDynamicProperties_ShouldReturnTrueWhenCostIsEqualToContainedAmount()
        {
            //Given
            var caster = new TestObjectEntity();
            caster.RawData = new Dictionary<string, object>();
            caster.SetProperty<LifeState>("LifeState", new LifeState
            {
                ActionPoints = 10
            });
            var data = new Dictionary<string, object>();
            data["propertyName"] = "LifeState";
            data["valueProperty"] = "ActionPoints";
            data["cost"] = 10;

            //When
            var validator = new CasterHasCostApValidator();
            var actual = validator.check(
                caster,
                null,
                data
            );

            //Then
            Assert.True(actual);
        }
    }
}