using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.State;
using FluentAssertions;
using Xunit;

namespace EventHorizon.Zone.System.ClientEntities.Tests.State
{
    public class ClientEntityInMemoryRepositoryTests
    {
        [Fact]
        public void ShouldReturnDefaultEntityWhenNoFoundInRepository()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var expected = default(ClientEntity);

            // When
            var repository = new ClientEntityInMemoryRepository();
            var actual = repository.Find(
                clientEntityId
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void ShouldReturnEntityWhenFoundInMap()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var expected = new ClientEntity(
                clientEntityId,
                new Dictionary<string, object>()
            );

            // When
            var repository = new ClientEntityInMemoryRepository();
            repository.Add(
                expected
            );
            var actual = repository.Find(
                clientEntityId
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void ShouldReturnLastEntityAddedWhenMultipleAreAddedWithTheSameClientEntityId()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var first = new ClientEntity(
                clientEntityId,
                new Dictionary<string, object>()
            );
            var expected = new ClientEntity(
                clientEntityId,
                new Dictionary<string, object>()
            );

            // When
            var repository = new ClientEntityInMemoryRepository();
            repository.Add(
                first
            );
            repository.Add(
                expected
            );
            var actual = repository.Find(
                clientEntityId
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void ShouldReturnAllEntitiesAddedWhenCallingAll()
        {
            // Given
            var expectedFirst = new ClientEntity(
                "1-client-entity-first-id",
                new Dictionary<string, object>()
            );
            var expectedSecond = new ClientEntity(
                "2-client-entity-second-id",
                new Dictionary<string, object>()
            );

            // When
            var repository = new ClientEntityInMemoryRepository();
            repository.Add(
                expectedFirst
            );
            repository.Add(
                expectedSecond
            );
            var actual = repository.All();

            // Then
            actual.Should()
                .Contain(
                    expectedFirst
                ).And.Contain(
                    expectedSecond
                );
        }

        [Fact]
        public void ShouldRemoveEntityWhenRemoveIsCalledForClientEntityId()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var expectedFirst = new ClientEntity(
                clientEntityId,
                new Dictionary<string, object>()
            );

            // When
            var repository = new ClientEntityInMemoryRepository();
            repository.Add(
                expectedFirst
            );
            var actual = repository.All();
            actual.Should()
                .NotBeEmpty();
            repository.Remove(
                clientEntityId
            );
            actual = repository.All();

            // Then
            actual.Should()
                .BeEmpty();
        }
    }
}