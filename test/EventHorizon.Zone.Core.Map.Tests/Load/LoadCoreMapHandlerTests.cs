namespace EventHorizon.Zone.Core.Map.Tests.Load
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.Core.Map.Load;
    using EventHorizon.Zone.Core.Map.State;

    using MediatR;

    using Moq;

    using Xunit;

    public class LoadCoreMapHandlerTests
    {
        [Fact]
        public async Task ShouldSendCreateMapWhenRequestIsHandled()
        {
            // Given
            var expected = new CreateMap();

            var mediatorMock = new Mock<IMediator>();
            var serverMapMock = new Mock<IServerMap>();

            // When
            var handler = new LoadCoreMapHandler(
                mediatorMock.Object,
                serverMapMock.Object
            );
            await handler.Handle(
                new LoadCoreMap(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<ClientActionGenericToAllEvent>(),
                    CancellationToken.None
                )
            );
        }
    }
}
