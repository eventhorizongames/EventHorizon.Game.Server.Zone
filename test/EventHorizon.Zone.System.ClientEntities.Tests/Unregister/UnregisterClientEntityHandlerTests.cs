namespace EventHorizon.Zone.System.ClientEntities.Tests.Unregister;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.Core.Events.Map.Cost;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Model.Client;
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
    [Theory, AutoMoqData]
    public async Task ReturnFalseEarlyWhenClientEntityIsNotFoundInRepository(
        // Given
        string globalId,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        UnregisterClientEntityHandler handler
    )
    {
        var expected = false;

        repositoryMock.Setup(
            mock => mock.Find(
                globalId
            )
        ).Returns(
            default(ClientEntity)
        );

        // When
        var actual = await handler.Handle(
            new(
                globalId
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task RemoveClientEntityToRepositoryFromRequest(
        // Given
        string globalId,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        UnregisterClientEntityHandler handler
    )
    {
        var clientEntity = new ClientEntity(
            globalId,
            new ConcurrentDictionary<string, object>()
        );

        repositoryMock.Setup(
            mock => mock.Find(
                globalId
            )
        ).Returns(
            clientEntity
        );

        // When
        var actual = await handler.Handle(
            new(
                globalId
            ),
            CancellationToken.None
        );

        // Then
        actual.Should()
            .BeTrue();
        repositoryMock.Verify(
            mock => mock.Remove(
                globalId
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task PublishClientActionOnSuccessfulRemove(
        // Given
        string globalId,
        [Frozen] Mock<IPublisher> publisherMock,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        UnregisterClientEntityHandler handler
    )
    {
        var clientEntity = new ClientEntity(
            globalId,
            new ConcurrentDictionary<string, object>()
        );

        repositoryMock.Setup(
            mock => mock.Find(
                globalId
            )
        ).Returns(
            clientEntity
        );

        // When
        var actual = await handler.Handle(
            new(
                globalId
            ),
            CancellationToken.None
        );

        // Then
        publisherMock.Verify(
            mock => mock.Publish(
                new ClientActionGenericToAllEvent(
                    "SERVER_CLIENT_ENTITY_DELETED_CLIENT_ACTION_EVENT",
                    new ClientEntityDeletedClientActionData(
                        globalId
                    )
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ChangeEdgeCostForNodesAtPositionWhenClientEnityHasDentityBoxProperty(
        // Given
        string clientEntityId,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        UnregisterClientEntityHandler handler
    )
    {
        var position = Vector3.Zero;
        var densityBox = Vector3.One;
        var cost = 500;
        var expected = new RemoveEdgeCostForNodesAtPosition(
            position,
            densityBox,
            cost
        );
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

        repositoryMock.Setup(
            mock => mock.Find(
                clientEntityId
            )
        ).Returns(
            clientEntity
        );

        // When
        await handler.Handle(
            new(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                expected,
                CancellationToken.None
            )
        );
        repositoryMock.Verify(
            mock => mock.Remove(
                clientEntityId
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ChangeEdgeCostForNodeAtPositionWhenClientEnityHasDentityBoxProperty(
        // Given
        string clientEntityId,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        UnregisterClientEntityHandler handler
    )
    {
        // Given
        var position = Vector3.Zero;
        var densityBox = Vector3.One;
        var cost = 500;
        var expected = new RemoveEdgeCostForNodeAtPosition(
            position,
            cost
        );
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

        repositoryMock.Setup(
            mock => mock.Find(
                clientEntityId
            )
        ).Returns(
            clientEntity
        );

        // When
        await handler.Handle(
            new UnregisterClientEntity(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                expected,
                CancellationToken.None
            )
        );
        repositoryMock.Verify(
            mock => mock.Remove(
                clientEntityId
            )
        );
    }
}
