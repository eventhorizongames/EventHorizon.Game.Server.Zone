namespace EventHorizon.Zone.System.Server.Scripts.Tests.State
{
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;
    using Xunit;

    public class ServerScriptDetailsInMemoryRepositoryTests
    {
        [Fact]
        public void TestShouldAddScriptDetailsWhenAddedById()
        {
            // Given
            var id = "script-id";
            var hash = "";
            var fileName = "file-name";
            var path = "path";
            var scriptString = "script-string";
            var scriptDetails = new ServerScriptDetails(
                id,
                hash,
                fileName,
                path,
                scriptString,
                null
            );
            var expected = new List<ServerScriptDetails>
            {
                scriptDetails,
            };

            // When
            var repository = new ServerScriptDetailsInMemoryRepository();
            repository.Add(
                id,
                scriptDetails
            );

            var actual = repository.Where(
                details => details.Id == id
            );

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
        }

        [Fact]
        public void ShouldReturnAddedScriptWhenUsingFindById()
        {
            // Given
            var id = "script-id";
            var hash = "";
            var fileName = "file-name";
            var path = "path";
            var scriptString = "script-string";
            var scriptDetails = new ServerScriptDetails(
                id,
                hash,
                fileName,
                path,
                scriptString,
                null
            );
            var expected = scriptDetails;

            // When
            var repository = new ServerScriptDetailsInMemoryRepository();
            repository.Add(
                id,
                scriptDetails
            );

            var actual = repository.Find(
                id
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void ShouldThrowExceptionOnFindWhenScriptsAreCleared()
        {
            // Given
            var id = "script-id";
            var hash = "";
            var fileName = "file-name";
            var path = "path";
            var scriptString = "script-string";
            var scriptDetails = new ServerScriptDetails(
                id,
                hash,
                fileName,
                path,
                scriptString,
                null
            );
            var expectedId = id;
            var expectedMessage = "Failed to find Server Script Details";

            // When
            var repository = new ServerScriptDetailsInMemoryRepository();
            repository.Add(
                id,
                scriptDetails
            );

            var existing = repository.Find(
                id
            );

            existing.Should().NotBeNull();

            repository.Clear();

            var actual = Assert.Throws<ServerScriptDetailsNotFound>(
                () => repository.Find(id)
            );

            // Then
            actual.ScriptId
                .Should().Be(expectedId);
            actual.Message
                .Should().Be(expectedMessage);
        }

        [Fact]
        public void ShouldUpdateExistingScriptDetailsWhenAddIsCalledWithSameId()
        {
            // Given
            var id = "script-id";
            var hash = "";
            var fileName = "file-name";
            var path = "path";
            var scriptString = "script-string";
            var scriptDetails = new ServerScriptDetails(
                id,
                hash,
                fileName,
                path,
                scriptString,
                null
            );
            var scriptString2 = "script-string";
            var scriptDetails2 = new ServerScriptDetails(
                id,
                hash,
                fileName,
                path,
                scriptString2,
                null
            );
            var expected = scriptDetails2;

            // When
            var repository = new ServerScriptDetailsInMemoryRepository();
            repository.Add(
                id,
                scriptDetails
            );

            var firstScript = repository.Find(
                id
            );
            firstScript.Should().Be(
                scriptDetails
            );

            repository.Add(
                id,
                scriptDetails2
            );
            var actual = repository.Find(
                id
            );

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
