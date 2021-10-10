namespace EventHorizon.Zone.System.Player.Tests.PopulateData
{
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.PopulateData;

    using FluentAssertions;

    using global::System.Collections.Concurrent;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Xunit;

    public class PopulatePlayerEntityConfigurationHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldNotSetPlayerPropertyWhenNotPlayerEntityType(
            // Given
            PopulatePlayerEntityConfigurationHandler handler
        )
        {
            var defaultEntity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            var notification = new PopulateEntityDataEvent
            {
                Entity = defaultEntity,
            };

            // When
            await handler.Handle(
                notification,
                CancellationToken.None
            );

            // Then
            defaultEntity.Data.Should().NotContainKey(
                "playerConfiguration"
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldSetPlayerConfigurationWhenEntityIsPlayer(
            // Given
            PopulatePlayerEntityConfigurationHandler handler
        )
        {
            var defaultEntity = new PlayerEntity
            {
                RawData = new ConcurrentDictionary<string, object>(),
            };
            var notification = new PopulateEntityDataEvent
            {
                Entity = defaultEntity,
            };

            // When
            await handler.Handle(
                notification,
                CancellationToken.None
            );

            // Then
            defaultEntity.Data.Should().ContainKey(
                "playerConfiguration"
            );
        }
    }
}
