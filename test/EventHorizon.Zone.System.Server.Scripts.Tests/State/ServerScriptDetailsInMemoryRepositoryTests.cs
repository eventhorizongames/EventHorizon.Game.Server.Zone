namespace EventHorizon.Zone.System.Server.Scripts.Tests.State
{
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using Xunit;

    public class ServerScriptDetailsInMemoryRepositoryTests
    {
        [Fact]
        public void TestShouldAddScriptDetailsWhenAddedById()
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