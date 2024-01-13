namespace EventHorizon.Zone.Core.Entity.Tests.Register;

using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Entity.Register;
using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using EventHorizon.Zone.Core.Set;

using MediatR;

using Moq;

using Xunit;

public class RegisterEntityHandlerTests
{
    [Theory, AutoMoqData]
    public async Task TestShouldAddEntityToRepositoryThenPublishEntityRegisteredEvent(
        // Given
        [Frozen] Mock<EntityRepository> entityRepositoryMock,
        [Frozen] Mock<IMediator> mediatorMock,
        RegisterEntityHandler handler
    )
    {
        var expectedEntity = new DefaultEntity();

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<SetEntityPropertyOverrideDataCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<IObjectEntity>(
                expectedEntity
            )
        );

        entityRepositoryMock.Setup(
            mock => mock.Add(
                It.IsAny<IObjectEntity>()
            )
        ).ReturnsAsync(
            expectedEntity
        );

        // When
        await handler.Handle(
            new RegisterEntityEvent
            {
                Entity = expectedEntity
            },
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Publish(
                new PrePopulateEntityDataEvent(
                    expectedEntity
                ),
                CancellationToken.None
            )
        );
        mediatorMock.Verify(
            mock => mock.Publish(
                new PopulateEntityDataEvent(
                    expectedEntity
                ),
                CancellationToken.None
            )
        );
        mediatorMock.Verify(
            mock => mock.Publish(
                new EntityRegisteredEvent
                {
                    Entity = expectedEntity
                },
                CancellationToken.None
            )
        );
    }
}
