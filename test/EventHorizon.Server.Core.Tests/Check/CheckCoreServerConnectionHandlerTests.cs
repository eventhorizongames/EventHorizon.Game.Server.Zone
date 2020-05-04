namespace EventHorizon.Server.Core.Tests.Check
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Check;
    using EventHorizon.Server.Core.Events.Check;
    using EventHorizon.Server.Core.Events.Register;
    using EventHorizon.Server.Core.Events.Stop;
    using EventHorizon.Server.Core.State;
    using MediatR;
    using Moq;
    using Xunit;

    public class CheckCoreServerConnectionHandlerTests
    {
        [Fact]
        public async Task ShouldResetCoreCheckStateWhenNotRegisteredWithCoreServer()
        {
            // Given

            var mediatorMock = new Mock<IMediator>();
            var serverCoreCheckStateMock = new Mock<ServerCoreCheckState>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForRegistrationWithCoreServer(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                false
            );

            // When
            var handler = new CheckCoreServerConnectionHandler(
                mediatorMock.Object,
                serverCoreCheckStateMock.Object
            );
            await handler.Handle(
                new CheckCoreServerConnection(),
                CancellationToken.None
            );

            // Then
            serverCoreCheckStateMock.Verify(
                mock => mock.Reset()
            );
        }

        [Fact]
        public async Task ShouldPublishRegisterWithCoreServerEventWhenNotRegisteredWithCoreServer()
        {
            // Given
            var expected = new RegisterWithCoreServer();

            var mediatorMock = new Mock<IMediator>();
            var serverCoreCheckStateMock = new Mock<ServerCoreCheckState>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForRegistrationWithCoreServer(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                false
            );

            // When
            var handler = new CheckCoreServerConnectionHandler(
                mediatorMock.Object,
                serverCoreCheckStateMock.Object
            );
            await handler.Handle(
                new CheckCoreServerConnection(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldCallCheckWhenCoreServerIsRegistered()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var serverCoreCheckStateMock = new Mock<ServerCoreCheckState>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForRegistrationWithCoreServer(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new CheckCoreServerConnectionHandler(
                mediatorMock.Object,
                serverCoreCheckStateMock.Object
            );
            await handler.Handle(
                new CheckCoreServerConnection(),
                CancellationToken.None
            );

            // Then
            serverCoreCheckStateMock.Verify(
                mock => mock.Check()
            );
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task ShouldStopRegisterAndResetWhenChecksDownIsEqualToOrGreaterThanMapRetries(
            int timesChecked
        )
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var serverCoreCheckStateMock = new Mock<ServerCoreCheckState>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new QueryForRegistrationWithCoreServer(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            serverCoreCheckStateMock.Setup(
                mock => mock.TimesChecked()
            ).Returns(
                timesChecked
            );

            // When
            var handler = new CheckCoreServerConnectionHandler(
                mediatorMock.Object,
                serverCoreCheckStateMock.Object
            );
            await handler.Handle(
                new CheckCoreServerConnection(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new StopCoreServerConnection(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new RegisterWithCoreServer(),
                    CancellationToken.None
                )
            );
            serverCoreCheckStateMock.Verify(
                mock => mock.Reset()
            );
            serverCoreCheckStateMock.Verify(
                mock => mock.Check(),
                Times.Never()
            );
        }
    }
}