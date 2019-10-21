using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;
using EventHorizon.Zone.System.Server.Scripts.State;
using Xunit;

namespace EventHorizon.Zone.System.Server.Scripts.Tests.State
{
    public class ServerScriptDetailsInMemoryRepositoryTests
    {
        [Fact]
        public async Task TestShouldAddScriptDetailsWhenAddedById()
        {
            // Given
            var scriptId = "script-id";
            var fileName = "file-name";
            var path = "path";
            var scriptString = "script-string";
            var expected = new ServerScriptDetails(
                scriptId,
                fileName,
                path,
                scriptString,
                null,
                null,
                null
            );

            // When
            var repository = new ServerScriptDetailsInMemoryRepository();
            repository.Add(
                scriptId,
                expected
            );

            var actual = repository.Where(
                details => details.Id == scriptId
            );

            // Then
            Assert.Collection(
                actual,
                details => Assert.Equal(expected, details)
            );
        }
    }
}