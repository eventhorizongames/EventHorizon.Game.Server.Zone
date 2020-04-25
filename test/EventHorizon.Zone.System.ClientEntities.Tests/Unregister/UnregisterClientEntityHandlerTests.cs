namespace EventHorizon.Zone.System.ClientEntities.Tests.Register
{
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.State;
    using EventHorizon.Zone.System.ClientEntities.Unregister;
    using FluentAssertions;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class UnregisterClientEntityHandlerTests
    {
        [Fact]
        public async Task ShouldReturnFalseEarlyWhenClientEntityIsNotFoundInRepository()
        {
            // Given
            var inputId = "id";
            var expected = false;

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

            // When
            var handler = new UnregisterClientEntityHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new UnregisterClientEntity(
                    inputId
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .Be(
                    expected
                );
        }

        [Fact]
        public async Task ShouldRemoveClientEntityToRepositoryFromRequest()
        {
            // Given
            var clientEntityId = "client-entity";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>()
            );

            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ClientEntityRepository>();

            repositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new UnregisterClientEntityHandler(
                mediatorMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new UnregisterClientEntity(
                    clientEntityId
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .BeTrue();
            repositoryMock.Verify(
                mock => mock.Remove(
                    clientEntityId
                )
            );
        }

        [Fact]
        public async Task ShouldChangeEdgeCostForNodesAtPositionWhenClientEnityHasDentityBoxProperty()
        {
            // Given
            var position = Vector3.Zero;
            var densityBox = Vector3.One;
            var cost = 500;
            var expected = new RemoveEdgeCostForNodesAtPosition(
                position,
                densityBox,
                cost
            );
            var clientEntityId = "client-entity-id";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense), true },
                        { nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox), densityBox },
                    }
                )
            );
            clientEntity.PopulateData<bool>(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
            );
            clientEntity.PopulateData<Vector3>(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
            );

            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ClientEntityRepository>();

            repositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new UnregisterClientEntityHandler(
                mediatorMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new UnregisterClientEntity(
                    clientEntityId
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

        [Fact]
        public async Task ShouldChangeEdgeCostForNodeAtPositionWhenClientEnityHasDentityBoxProperty()
        {
            // Given
            var position = Vector3.Zero;
            var densityBox = Vector3.One;
            var cost = 500;
            var expected = new RemoveEdgeCostForNodeAtPosition(
                position,
                cost
            );
            var clientEntityId = "client-entity-id";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense), true },
                    }
                )
            );
            clientEntity.PopulateData<bool>(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
            );

            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ClientEntityRepository>();

            repositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new UnregisterClientEntityHandler(
                mediatorMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new UnregisterClientEntity(
                    clientEntityId
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