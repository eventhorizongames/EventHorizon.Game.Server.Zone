namespace EventHorizon.Game.Tests.Registered
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Model;
    using EventHorizon.Game.Registered;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class CheckSkillsEntityRegisteredHandlerTests
    {
        [Fact]
        public async Task ShouldReturnWithoutDoingAnythingWhenEntityIsNotPlayer()
        {
            // Given
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );

            // When
            var handler = new CheckSkillsEntityRegisteredHandler();
            await handler.Handle(
                new EntityRegisteredEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );

            // Then
            entity.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldAddNewSkilLStateDetailsWhenEscapeOfCapturesSkillIsNotInSkillMap()
        {
            // Given
            var entity = new PlayerEntity
            {
                Type = EntityType.PLAYER,
                RawData = new ConcurrentDictionary<string, object>()
            };
            entity = entity.SetProperty(
                SkillState.PROPERTY_NAME,
                SkillState.NEW
            );

            var expected = new SkillStateDetails
            {
                Id = SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID,
            };

            // When
            var handler = new CheckSkillsEntityRegisteredHandler();
            await handler.Handle(
                new EntityRegisteredEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );
            var actual = entity.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            ).SkillMap.List.FirstOrDefault(
                a => a.Id == SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
