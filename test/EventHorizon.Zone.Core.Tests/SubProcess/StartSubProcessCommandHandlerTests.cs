namespace EventHorizon.Zone.Core.Tests.SubProcess
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.SubProcess;
    using EventHorizon.Zone.Core.SubProcess;

    using FluentAssertions;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class StartSubProcessCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnNotValidErrorWhenSubProcessStartIsNull()
        {
            // Given
            var expectedErrorCode = "SUB_PROCESS_START_ERROR";
            var applicationFullName = Path.Combine(
                "SubProcess"
            );

            var loggerMock = new Mock<ILogger<StartSubProcessCommandHandler>>();

            // When
            var handler = new StartSubProcessCommandHandler(
                loggerMock.Object
            );
            var actual = await handler.Handle(
                new StartSubProcessCommand(
                    applicationFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expectedErrorCode);
        }
    }
}
