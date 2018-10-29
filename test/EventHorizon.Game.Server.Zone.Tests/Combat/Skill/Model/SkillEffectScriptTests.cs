using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Combat.Skill.Model
{
    public class SkillEffectScriptTests
    {
        [Fact]
        public async Task TestRun_ServerFreezeEntity()
        {
            //Given
            var casterId = 1001L;
            var targetId = 2002L;
            var expectedCasterEvent = new ClientSkillActionEvent
            {
                Action = "casterId",
                Data = new
                {
                CasterId = casterId
                }
            };
            var data = new Dictionary<string, object>();
            var casterMock = new Mock<IObjectEntity>();
            casterMock.Setup(a => a.Id).Returns(casterId);
            var targetMock = new Mock<IObjectEntity>();
            targetMock.Setup(a => a.Id).Returns(targetId);

            //When
            var effectScript = new SkillEffectScript
            {
                ServerScriptFile = "ServerFreezeEntity.csx"
            };
            effectScript.CreateScript(
                @"C:\Repos\EventHorizon.Game\EventHorizon.Game.Server.Zone\src\EventHorizon.Plugin.Zone.System.Combat\Assets\Scripts\Effects"
            );
            var actual = await effectScript.Run(
                casterMock.Object,
                targetMock.Object,
                data
            );

            //Then
            Assert.Collection(actual, a =>
            {
                Assert.Equal(expectedCasterEvent.Action, a.Action);
            });
        }
    }
    public struct TestEvent : INotification
    {
        public string TestValue { get; set; }
    }
}