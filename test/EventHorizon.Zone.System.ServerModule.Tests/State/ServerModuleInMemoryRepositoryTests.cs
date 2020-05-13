namespace EventHorizon.Zone.System.ServerModule.Tests.State
{
    using EventHorizon.Zone.System.ServerModule.Model;
    using EventHorizon.Zone.System.ServerModule.State;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using Xunit;

    public class ServerModuleInMemoryRepositoryTests
    {
        [Fact]
        public void ShouldAddScriptToRepositoryWhenNotAlreadyInRepository()
        {
            // Given
            var script = new ServerModuleScripts
            {
                Name = "script-name"
            };
            var expected = new List<ServerModuleScripts>
            {
                script
            };

            // When
            var repository = new ServerModuleInMemoryRepository();
            repository.All()
                .Should().BeEmpty();
            repository.Add(
                script
            );

            var actual = repository.All();

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
        }

        [Fact]
        public void ShouldReplaceScriptWhenAddingNewScriptWithSameName()
        {
            // Given
            var scriptName = "script-name";
            var script1 = new ServerModuleScripts
            {
                Name = scriptName,
                InitializeScript = "script1-InitializeScript",
            };
            var script2 = new ServerModuleScripts
            {
                Name = scriptName,
                InitializeScript = "script2-InitializeScript",
            };
            var expected = new List<ServerModuleScripts>
            {
                script1,
            };

            // When
            var repository = new ServerModuleInMemoryRepository();
            repository.Add(
                script2
            );
            repository.Add(
                script1
            );

            var actual = repository.All();

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
        }
    }
}
