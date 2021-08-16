namespace EventHorizon.Zone.Core.Entity.Tests.Update
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Update;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Events.Entity.Action;
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.Client;

    using MediatR;

    using Moq;

    using Xunit;

    public class EntityActionEventPropertyChangedHandlerTests
    {
        [Fact]
        public async Task ShouldPublishClientActionToAllEventWhenEventActionIsAPropertyChanged()
        {
            // Given
            var action = EntityAction.PROPERTY_CHANGED;
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new EntityActionEventPropertyChangedHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new EntityActionEvent(
                    action,
                    entity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    ClientActionEntityClientChangedToAllEvent.Create(
                        new EntityChangedData(
                            entity
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Theory]
        [ClassData(typeof(EntityActionsTestDataGenerator))]
        public async Task ShouldNotPublishClientActionToAllEventWhenEventActionIsNotPropertyChanged(
            // Given
            EntityAction action
        )
        {
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new EntityActionEventPropertyChangedHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new EntityActionEvent(
                    action,
                    entity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<ClientActionGenericToAllEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        public class EntityActionsTestDataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] { EntityAction.ADD },
                new object[] { EntityAction.POSITION },
                new object[] { EntityAction.REMOVE },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
