namespace EventHorizon.Zone.System.ClientEntities.Tests.Register
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Register;
    using EventHorizon.Zone.System.ClientEntities.State;
    using EventHorizon.Zone.System.ClientEntities.Update;
    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class RegisterClientEntityCommandHandlerTests
    {
        [Fact]
        public async Task ShouldAddClientEntityToRepositoryFromRequest()
        {
            // Given
            var expected = new ClientEntity(
                "client-entity",
                new ConcurrentDictionary<string, object>()
            );

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

            // When
            var handler = new RegisterClientEntityCommandHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
            await handler.Handle(
                new RegisterClientEntityCommand(
                    expected
                ),
                CancellationToken.None
            );

            // Then
            clientEntityRepositoryMock.Verify(
                mock => mock.Add(
                    expected
                )
            );
        }

        [Fact]
        public async Task ShouldSend()
        {
            // Given
            var entity = new ClientEntity(
                "client-entity",
                new ConcurrentDictionary<string, object>()
            );
            var expected = new SetClientEntityNodeDensity(
                entity
            );

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

            // When
            var handler = new RegisterClientEntityCommandHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
            await handler.Handle(
                new RegisterClientEntityCommand(
                    entity
                ),
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
    }
}