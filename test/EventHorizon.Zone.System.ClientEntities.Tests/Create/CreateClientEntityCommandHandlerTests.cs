namespace EventHorizon.Zone.System.ClientEntities.Tests.Create
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.ClientEntities.Create;
    using EventHorizon.Zone.System.ClientEntities.Model;

    using FluentAssertions;

    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class CreateClientEntityCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnUpdatedEntityWhenSuccessfulySaved()
        {
            // Given
            var clientEntityPath = "client-entity-path";
            var clientEntityId = "client-entity-id";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>()
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientEntityPath
            ).Returns(
                clientEntityPath
            );

            // When
            var handler = new CreateClientEntityCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new CreateClientEntityCommand(
                    clientEntity
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            actual.ErrorCode.Should().BeEmpty();
            actual.ClientEntity.ClientEntityId.Should().NotBe(clientEntityId);
            actual.ClientEntity.RawData.Should().ContainKey(
                ClientEntityConstants.METADATA_FILE_FULL_NAME
            );
        }
    }
}
