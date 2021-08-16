namespace EventHorizon.Zone.System.Client.Scripts.Tests.State
{
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.State;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    using Xunit;

    public class ClientScriptInMemoryRepositoryTests
    {
        [Fact]
        public void ShouldAddScriptToRepositoryWhenNotAlreadyInRepository()
        {
            // Given
            var newClientScript = ClientScript.Create(
                ClientScriptType.CSharp,
                "path",
                "file-name",
                "script-string"
            );
            var expected = new List<ClientScript>
            {
                newClientScript
            };

            // When
            var repository = new ClientScriptInMemoryRepository();
            repository.All().Should().BeEmpty();
            repository.Add(
                newClientScript
            );
            var actual = repository.All();

            // Then
            actual.Should()
                .BeEquivalentTo(
                    expected
                );
        }

        [Fact]
        public void ShouldUpdateScriptToRepositoryWhenNotAlreadyInRepository()
        {
            // Given
            var existingClientScript = ClientScript.Create(
                ClientScriptType.JavaScript,
                "path",
                "file-name",
                "script-string"
            );
            var newClientScript = ClientScript.Create(
                ClientScriptType.CSharp,
                "path",
                "file-name",
                "script-string"
            );
            var expected = new List<ClientScript>
            {
                newClientScript
            };

            // When
            var repository = new ClientScriptInMemoryRepository();
            repository.Add(
                existingClientScript
            );
            repository.All().Should().BeEquivalentTo(
                new List<ClientScript>
                {
                    existingClientScript
                }
            );
            repository.Add(
                newClientScript
            );
            var actual = repository.All();

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
        }
    }
}
