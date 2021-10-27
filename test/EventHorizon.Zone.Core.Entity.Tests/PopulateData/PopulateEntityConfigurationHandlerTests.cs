namespace EventHorizon.Zone.Core.Entity.Tests.PopulateData
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Entity.PopulateData;
    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;

    using FluentAssertions;

    using Xunit;

    public class PopulateEntityConfigurationHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldSetEntityPropertyWhenEventIsHandled(
            // Given
            [Frozen] ObjectEntityConfiguration entityConfiguration,
            PopulateEntityConfigurationHandler handler
        )
        {
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );
            var notification = new PopulateEntityDataEvent
            {
                Entity = entity,
            };

            // When
            await handler.Handle(
                notification,
                CancellationToken.None
            );

            // Then
            entity.Data.Should().ContainKey(
                "entityConfiguration"
            );
            entity.Data["entityConfiguration"]
                .Should().Be(
                    entityConfiguration
                );
        }
    }
}
