using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.State;
using Xunit;

namespace EventHorizon.Zone.System.Server.Scripts.Tests.State
{
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
            Assert.Equal(
                expected,
                actualException.Message 
            );
            Assert.Equal(
                expectedScriptId,
                actualException.ScriptId
            );
        }

        public struct TestServerScript : ServerScript
        {
            public string Id { get; set; }

            public Task<ServerScriptResponse> Run(
                ServerScriptServices services,
                IDictionary<string, object> data
            )
            {
                throw new global::System.NotImplementedException();
            }
        }
    }
}