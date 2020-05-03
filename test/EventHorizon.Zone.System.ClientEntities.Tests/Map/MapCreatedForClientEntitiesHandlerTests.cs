namespace EventHorizon.Zone.System.ClientEntities.Tests.Map
{
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.System.ClientEntities.Map;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.State;
    using EventHorizon.Zone.System.ClientEntities.Update;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class MapCreatedForClientEntitiesHandlerTests
    {
        [Fact]
        public async Task ShouldSetNodeDensityWhenRepositoryContainsClientEntities()
        {
            // Given
            var expected = new ClientEntity();

            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ClientEntityRepository>();

            repositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                new List<ClientEntity>
                {
                    expected
                }
            );
            // When
            var handler = new MapCreatedForClientEntitiesHandler(
                mediatorMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new MapCreatedEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Setup(
                mock => mock.Send(
                    new SetClientEntityNodeDensity(
                        expected
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}