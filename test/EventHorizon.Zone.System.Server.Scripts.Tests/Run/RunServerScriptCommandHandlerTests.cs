using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;
using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.Run;
using EventHorizon.Zone.System.Server.Scripts.System;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Server.Scripts.Tests.Run
{
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
                    data
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
                    data
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
                    data
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