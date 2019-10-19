using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Script.Run
{
    public class RunBehaviorScriptHandlerTests
    {
        [Fact]
        public async Task TestShouldSendRunServerScriptCommandWhenHandlingRunCommand()
        {
            // Given
            var expected = BehaviorNodeStatus.SUCCESS;
            var expectedMessage = string.Empty;
            var scriptId = "script-id";
            var actor = new DefaultEntity();

            var loggerMock = new Mock<ILogger<RunBehaviorScriptHandler>>();
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new BehaviorScriptResponse(
                    expected
                )
            );

            // When
            var runBehaviorScriptHandler = new RunBehaviorScriptHandler(
                loggerMock.Object,
                mediatorMock.Object
            );
            var actual = await runBehaviorScriptHandler.Handle(
                new RunBehaviorScript(
                    actor,
                    scriptId
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual.Status
            );
            Assert.True(
                actual.Success
            );
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        command => command.Id == scriptId
                            &&
                            command.Data.ContainsKey("Actor")
                            &&
                            command.Data["Actor"].Equals(
                                actor
                            )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldReturnErrorStatusWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = BehaviorNodeStatus.ERROR;
            var scriptId = "script-id";
            var actor = new DefaultEntity();

            var loggerMock = new Mock<ILogger<RunBehaviorScriptHandler>>();
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception(
                    "error"
                )
            );

            // When
            var runBehaviorScriptHandler = new RunBehaviorScriptHandler(
                loggerMock.Object,
                mediatorMock.Object
            );
            var actual = await runBehaviorScriptHandler.Handle(
                new RunBehaviorScript(
                    actor,
                    scriptId
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual.Status
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        command => command.Id == scriptId
                            &&
                            command.Data.ContainsKey("Actor")
                            &&
                            command.Data["Actor"].Equals(
                                actor
                            )
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}