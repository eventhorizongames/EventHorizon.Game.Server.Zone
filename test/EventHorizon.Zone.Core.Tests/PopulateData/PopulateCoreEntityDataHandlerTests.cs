using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.Movement;
using EventHorizon.Zone.Core.PopulateData;
using FluentAssertions;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.PopulateData
{
    public class PopulateCoreEntityDataHandlerTests
    {
        [Fact]
        public async Task ShouldPopulateLocationStateDataWhenRequestIsHandled()
        {
            // Given
            var expected = new LocationState
            {
                CanMove = true,
                ZoneTag = "zone-tag",
            };
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    { 
                        { LocationState.PROPERTY_NAME, expected}
                    }
                )
            );

            // When
            var handler = new PopulateCoreEntityDataHandler();
            await handler.Handle(
                new PopulateEntityDataEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );

            // Then
            entity.As<IObjectEntity>().ContainsProperty(
                LocationState.PROPERTY_NAME
            ).Should().BeTrue();
            entity.GetProperty<LocationState>(
                    LocationState.PROPERTY_NAME
                ).Should().Be(
                    expected
                );
        }

        [Fact]
        public async Task ShouldPopulateLocationStateDataWithNewWhenNotInRawData()
        {
            // Given
            var expected = LocationState.NEW;
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );

            // When
            var handler = new PopulateCoreEntityDataHandler();
            await handler.Handle(
                new PopulateEntityDataEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );

            // Then
            entity.As<IObjectEntity>().ContainsProperty(
                LocationState.PROPERTY_NAME
            ).Should().BeTrue();
            entity.GetProperty<LocationState>(
                    LocationState.PROPERTY_NAME
                ).Should().Be(
                    expected
                );
        }

        [Fact]
        public async Task ShouldPopulateMovementStateDataWhenRequestIsHandled()
        {
            // Given
            var expected = new MovementState
            {
                Speed = 100,
            };
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    { 
                        { MovementState.PROPERTY_NAME, expected}
                    }
                )
            );

            // When
            var handler = new PopulateCoreEntityDataHandler();
            await handler.Handle(
                new PopulateEntityDataEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );

            // Then
            entity.As<IObjectEntity>().ContainsProperty(
                MovementState.PROPERTY_NAME
            ).Should().BeTrue();
            entity.GetProperty<MovementState>(
                    MovementState.PROPERTY_NAME
                ).Should().Be(
                    expected
                );
        }

        [Fact]
        public async Task ShouldPopulateMovementStateDataWithNewWhenNotInRawData()
        {
            // Given
            var expected = MovementState.NEW;
            var entity = new DefaultEntity(
                new ConcurrentDictionary<string, object>()
            );

            // When
            var handler = new PopulateCoreEntityDataHandler();
            await handler.Handle(
                new PopulateEntityDataEvent
                {
                    Entity = entity,
                },
                CancellationToken.None
            );

            // Then
            entity.As<IObjectEntity>().ContainsProperty(
                MovementState.PROPERTY_NAME
            ).Should().BeTrue();
            entity.GetProperty<MovementState>(
                    MovementState.PROPERTY_NAME
                ).Should().Be(
                    expected
                );
        }
    }
}