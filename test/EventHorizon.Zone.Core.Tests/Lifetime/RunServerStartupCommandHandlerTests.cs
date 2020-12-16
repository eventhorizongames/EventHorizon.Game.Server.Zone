namespace EventHorizon.Zone.Core.Tests.Lifetime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Lifetime;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using FluentAssertions;
    using MediatR;
    using Moq;
    using Xunit;

    public class RunServerStartupCommandHandlerTests
    {
        [Fact]
        public async Task ShouldSendOnStartupCommandsFoundInReferencedAssembiles()
        {
            // Given
            var expected = new OnStartupTestingCommand();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<OnServerStartupCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new OnServerStartupResult(
                    true
                )
            );

            // When
            var handler = new RunServerStartupCommandHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new RunServerStartupCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenOnStartupCommandReturnsFalseSuccess()
        {
            // Given
            var errorCode = "error-code";
            var expected = $"Failed '{nameof(OnStartupTestingCommand)}' with ErrorCode: {errorCode}";

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new OnStartupTestingCommand(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new OnServerStartupResult(
                    false,
                    errorCode
                )
            );

            // When
            var handler = new RunServerStartupCommandHandler(
                mediatorMock.Object
            );
            //act
            async Task handlerAction() => await handler.Handle(
                new RunServerStartupCommand(),
                CancellationToken.None
            );
            //assert
            var actual = await Assert.ThrowsAsync<SystemException>(
                handlerAction
            );

            // Then
            actual.Message
                .Should().BeEquivalentTo(
                    expected
                );
        }

        [Fact]
        public async Task ShouldThrowSystemExceptionWhenGeneralExceptionIsThrown()
        {
            // Given
            var expected = $"Failed '{nameof(OnStartupTestingCommand)}' with Exception.";

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new OnStartupTestingCommand(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception(
                    "Failed"
                )
            );

            // When
            var handler = new RunServerStartupCommandHandler(
                mediatorMock.Object
            );
            //act
            async Task handlerAction() => await handler.Handle(
                new RunServerStartupCommand(),
                CancellationToken.None
            );
            //assert
            var actual = await Assert.ThrowsAsync<SystemException>(
                handlerAction
            );

            // Then
            actual.Message
                .Should().BeEquivalentTo(
                    expected
                );
        }

        public struct OnStartupTestingCommand
            : OnServerStartupCommand
        {

        }
    }
}
