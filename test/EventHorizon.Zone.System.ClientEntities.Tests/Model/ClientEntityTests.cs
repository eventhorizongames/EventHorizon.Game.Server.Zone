using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.ClientEntities.Model;
using FluentAssertions;
using System.Collections.Concurrent;
using Xunit;

namespace EventHorizon.Zone.System.ClientEntities.Tests.Model
{
    public class ClientEntityTests
    {
        [Fact]
        public void ShouldCorrectlyDefaultValuesWhenInitiallyCreated()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var rawData = new ConcurrentDictionary<string, object>();
            var expectedId = -1;
            var expectedType = EntityType.OTHER;

            // When
            var actual = new ClientEntity(
                clientEntityId,
                rawData
            );

            // Then
            actual.Id
                .Should()
                .Be(
                    expectedId
                );
            actual.Type
                .Should()
                .Be(
                    expectedType
                );
        }
    }
}