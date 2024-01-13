
namespace EventHorizon.Zone.System.ClientEntities.Tests.Update;

using EventHorizon.Zone.Core.Events.Map.Cost;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Update;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;
using global::System.Numerics;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class SetClientEntityNodeDensityHandlerTests
{
    [Fact]
    public async Task ShouldChangeEdgeCostForNodesAtPositionWhenClientEnityHasDentityBoxProperty()
    {
        // Given
        var position = Vector3.Zero;
        var densityBox = Vector3.One;
        var cost = 500;
        var expected = new ChangeEdgeCostForNodesAtPositionCommand(
            position,
            densityBox,
            cost
        );
        var clientEntity = new ClientEntity(
            "client-entity",
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

        // When
        var handler = new SetClientEntityNodeDensityHandler(
            mediatorMock.Object
        );
        await handler.Handle(
            new SetClientEntityNodeDensity(
                clientEntity
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
        var expected = new ChangeEdgeCostForNodeAtPosition(
            position,
            cost
        );
        var clientEntity = new ClientEntity(
            "client-entity",
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

        // When
        var handler = new SetClientEntityNodeDensityHandler(
            mediatorMock.Object
        );
        await handler.Handle(
            new SetClientEntityNodeDensity(
                clientEntity
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
