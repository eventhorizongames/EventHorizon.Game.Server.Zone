namespace EventHorizon.Zone.System.Server.Scripts.Tests.Run
{
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Run;
    using EventHorizon.Zone.System.Server.Scripts.System;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class RunServerScriptCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnScriptResponseWhenFoundInRepository()
        {
            // Given
            var scriptId = "script-id";
            var serverScriptMock = new Mock<ServerScript>();
            var data = new Dictionary<string, object>();
            var expected = new TestServerScriptResponse();

            var loggerMock = new Mock<ILogger<RunServerScriptCommandHandler>>();
            var serverScriptRepositoryMock = new Mock<ServerScriptRepository>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

            serverScriptRepositoryMock.Setup(
                mock => mock.Find(
                    scriptId
                )
            ).Returns(
                serverScriptMock.Object
            );

            serverScriptMock.Setup(
                mock => mock.Run(
                    serverScriptServicesMock.Object,
                    It.IsAny<ServerScriptData>()
                )
            ).ReturnsAsync(
                expected
            );

            // When
            var handler = new RunServerScriptCommandHandler(
                loggerMock.Object,
                serverScriptRepositoryMock.Object,
                serverScriptServicesMock.Object
            );
            var actual = (TestServerScriptResponse)await handler.Handle(
                new RunServerScriptCommand(
                    scriptId,
                    data
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task TestShouldReturnServerScriptNotFoundWhenScriptRunThrowsScriptNotFound()
        {
            // Given
            var scriptId = "script-id";
            var serverScriptMock = new Mock<ServerScript>();
            var data = new Dictionary<string, object>();
            var expected = new SystemServerScriptResponse(
                false,
                "server_script_not_found"
            );

            var loggerMock = new Mock<ILogger<RunServerScriptCommandHandler>>();
            var serverScriptRepositoryMock = new Mock<ServerScriptRepository>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

            serverScriptRepositoryMock.Setup(
                mock => mock.Find(
                    scriptId
                )
            ).Returns(
                serverScriptMock.Object
            );

            serverScriptMock.Setup(
                mock => mock.Run(
                    serverScriptServicesMock.Object,
                    It.IsAny<ServerScriptData>()
                )
            ).Throws(
                new ServerScriptNotFound(
                    scriptId,
                    "Script not found"
                )
            );

            // When
            var handler = new RunServerScriptCommandHandler(
                loggerMock.Object,
                serverScriptRepositoryMock.Object,
                serverScriptServicesMock.Object
            );
            var actual = (SystemServerScriptResponse)await handler.Handle(
                new RunServerScriptCommand(
                    scriptId,
                    data
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task TestShouldReturnGeneralServerErrorWhenScriptRunThrowsGeneralError()
        {
            // Given
            var scriptId = "script-id";
            var serverScriptMock = new Mock<ServerScript>();
            var data = new Dictionary<string, object>();
            var expected = new SystemServerScriptResponse(
                false,
                "general_server_script_error"
            );

            var loggerMock = new Mock<ILogger<RunServerScriptCommandHandler>>();
            var serverScriptRepositoryMock = new Mock<ServerScriptRepository>();
            var serverScriptServicesMock = new Mock<ServerScriptServices>();

            serverScriptRepositoryMock.Setup(
                mock => mock.Find(
                    scriptId
                )
            ).Returns(
                serverScriptMock.Object
            );

            serverScriptMock.Setup(
                mock => mock.Run(
                    serverScriptServicesMock.Object,
                    It.IsAny<ServerScriptData>()
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var handler = new RunServerScriptCommandHandler(
                loggerMock.Object,
                serverScriptRepositoryMock.Object,
                serverScriptServicesMock.Object
            );
            var actual = (SystemServerScriptResponse)await handler.Handle(
                new RunServerScriptCommand(
                    scriptId,
                    data
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        public struct TestServerScriptResponse : ServerScriptResponse
        {
            public bool Success { get; }

            public string Message { get; }
        }
    }
}
