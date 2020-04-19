namespace EventHorizon.Zone.System.ClientEntities.Tests.Register
{
    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using Xunit;
    using Moq;
    using MediatR;
    using EventHorizon.Zone.System.ClientEntities.State;
    using EventHorizon.Zone.System.ClientEntities.Register;
    using global::System.Threading;
    using global::System.Numerics;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Events.Map.Cost;

    public class RegisterClientEntityCommandHandlerTests
    {
        [Fact]
        public async Task ShouldAddClientEntityToRepositoryFromRequest()
        {
            // Given
            var expected = new ClientEntity(
                "client-entity",
                new Dictionary<string, object>()
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
                new Dictionary<string, object>
                {
                    { nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense), true },
                    { nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox), densityBox },
                }
            );
            clientEntity.PopulateData<bool>(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
            );
            clientEntity.PopulateData<Vector3>(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
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
            var expected = new ChangeEdgeCostForNodeAtPositionCommand(
                position,
                cost
            );
            var clientEntity = new ClientEntity(
                "client-entity",
                new Dictionary<string, object>
                {
                    { nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense), true },
                }
            );
            clientEntity.PopulateData<bool>(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
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
}