using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Services;
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
                Action = "freeze_entity",
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
            var scriptServicesMock = new Mock<IScriptServices>();
            var mediatorMock = new Mock<IMediator>();
            scriptServicesMock.Setup(a => a.Mediator).Returns(mediatorMock.Object);
            var skill = new SkillInstance();

            //When
            var effectScript = new SkillEffectScript
            {
                ScriptFile = "FreezeCaster.csx"
            };
            effectScript.CreateScript(
                System.IO.Path.Combine(
                    "Combat",
                    "Skill",
                    "Model"
                )
            );
            var actual = await effectScript.Run(
                scriptServicesMock.Object,
                casterMock.Object,
                targetMock.Object,
                skill,
                data,
                null
            );

            //Then
            Assert.Collection(actual.ActionList, a =>
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