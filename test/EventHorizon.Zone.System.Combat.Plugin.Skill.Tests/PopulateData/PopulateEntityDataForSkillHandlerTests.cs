namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.PopulateData
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.PopulateData;

    using FluentAssertions;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Xunit;

    public class PopulateEntityDataForSkillHandlerTests
    {
        [Fact]
        public async Task ShouldAddSkillStateWhenListIsNull()
        {
            // Given
            var expected = SkillState.NEW;

            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );

            // When
            var handler = new PopulateEntityDataForSkillHandler();
            await handler.Handle(
                new Core.Events.Entity.Data.PopulateEntityDataEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );
            var actual = entity.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldNotSetToNewPropertyWhenValidSkillStateAlreadyExists()
        {
            // Given
            var skillMap = new SkillStateMap
            {
                List = new List<SkillStateDetails>()
            };
            var expected = new SkillState
            {
                // This makes it valid
                SkillMap = skillMap,
            };

            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            SkillState.PROPERTY_NAME,
                            new SkillState
                            {
                                SkillMap = skillMap,
                            }
                        }
                    }
                )
            );

            // When
            var handler = new PopulateEntityDataForSkillHandler();
            await handler.Handle(
                new Core.Events.Entity.Data.PopulateEntityDataEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );
            var actual = entity.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task ShouldSetToNewPropertyWhenValidSkillStateAlreadyExistsAndIsNullist()
        {
            // Given
            var skillMap = new SkillStateMap
            {
                List = null,
            };
            var expected = SkillState.NEW;

            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            SkillState.PROPERTY_NAME,
                            new SkillState
                            {
                                SkillMap = skillMap,
                            }
                        }
                    }
                )
            );

            // When
            var handler = new PopulateEntityDataForSkillHandler();
            await handler.Handle(
                new Core.Events.Entity.Data.PopulateEntityDataEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );
            var actual = entity.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
