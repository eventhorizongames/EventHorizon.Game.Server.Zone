using System.Collections.Generic;
using System.Threading.Tasks;
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