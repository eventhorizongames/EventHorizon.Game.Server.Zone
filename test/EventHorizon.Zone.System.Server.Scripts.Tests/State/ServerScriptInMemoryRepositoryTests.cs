namespace EventHorizon.Zone.System.Server.Scripts.Tests.State
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using Xunit;
    using FluentAssertions;

    public class ServerScriptInMemoryRepositoryTests
    {
        [Fact]
        public void TestShouldBeFindScriptBasedOnIdWhenAddedToRepository()
        {
            // Given
            var scriptId = "script-id";
            var serverScript = new TestServerScript
            {
                Id = scriptId
            };
            var expected = serverScript;

            // When
            var repository = new ServerScriptInMemoryRepository();

            repository.Add(
                serverScript
            );

            var actual = repository.Find(
                scriptId
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldThrowNotFoundExceptionWhenFindScriptIsCalledWithScriptId()
        {
            // Given
            var scriptId = "script-id";
            var expected = "ServerScriptInMemoryRepository did not find the script.";
            var expectedScriptId = scriptId;

            // When
            var repository = new ServerScriptInMemoryRepository();
            Func<ServerScript> action = () => repository.Find(
                scriptId
            );

            // Then
            var actualException = Assert.Throws<ServerScriptNotFound>(
                action
            );

            actualException.Message
                .Should().Be(expected);
            actualException.ScriptId
                .Should().Be(expectedScriptId);
        }

        public struct TestServerScript 
            : ServerScript
        {
            public string Id { get; set; }
            public IEnumerable<string> Tags { get; }

            public Task<ServerScriptResponse> Run(
                ServerScriptServices services,
                IDictionary<string, object> data
            )
            {
                throw new global::System.NotImplementedException();
            }

            public Task<ServerScriptResponse> Run(ServerScriptServices services, ServerScriptData data)
            {
                throw new NotImplementedException();
            }
        }
    }
}