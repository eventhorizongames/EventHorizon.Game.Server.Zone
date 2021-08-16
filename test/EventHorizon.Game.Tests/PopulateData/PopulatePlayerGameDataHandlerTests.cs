using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Game.Model;
using EventHorizon.Game.PopulateData;
using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;

using FluentAssertions;

using Xunit;
namespace EventHorizon.Game.Tests.PopulateData
{
    public class PopulatePlayerGameDataHandlerTests
    {
        [Fact]
        public async Task ShouldDoNothingWhenNotPlayer()
        {
            // Given
            var entity = new DefaultEntity();
            var expected = entity;

            // When
            var handler = new PopulatePlayerGameDataHandler();
            await handler.Handle(
                new PopulateEntityDataEvent
                {
                    Entity = entity
                },
                CancellationToken.None
            );

            // Then
            entity.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldSetNewGamePlayerCpatureStateWhenTypeIsPlayer()
        {
            // Given
            var entity = new PlayerEntity
            {
                Type = EntityType.PLAYER,
                RawData = new ConcurrentDictionary<string, object>(),
            };
            var expected = GamePlayerCaptureState.New();

            // When
            var handler = new PopulatePlayerGameDataHandler();
            await handler.Handle(
                new PopulateEntityDataEvent
                {
                    Entity = entity
                },
                CancellationToken.None
            );
            var actual = (GamePlayerCaptureState)entity.Data[GamePlayerCaptureState.PROPERTY_NAME];

            // Then
            actual.Captures
                .Should().Be(expected.Captures);
            actual.CompanionsCaught
                .Should().BeEquivalentTo(expected.CompanionsCaught);
            actual.EscapeCaptureTime
                .Should().Be(expected.EscapeCaptureTime);
            actual.ShownFiveSecondMessage
                .Should().Be(expected.ShownFiveSecondMessage);
            actual.ShownTenSecondMessage
                .Should().Be(expected.ShownTenSecondMessage);
        }
    }
}
