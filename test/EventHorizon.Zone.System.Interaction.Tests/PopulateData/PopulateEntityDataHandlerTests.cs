namespace EventHorizon.Zone.System.Interaction.Tests.PopulateData
{
    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Plugin.Zone.Interaction.PopulateData;
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Interaction.Model;

    using Xunit;

    public class PopulateEntityDataHandlerTests
    {
        [Fact]
        public async Task TestShouldAddInteractionStateProperty()
        {
            // Given
            var expected = new InteractionState
            {
                Active = true,

                List = null
            };
            var data = new ConcurrentDictionary<string, object>();
            var entity = new DefaultEntity(data);

            // When
            var handler = new PopulateEntityDataHandler();
            await handler.Handle(
                new PopulateEntityDataEvent
                {
                    Entity = entity
                },
                CancellationToken.None
            );
            var actual = entity.GetProperty<InteractionState>(
                "interactionState"
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}
