namespace EventHorizon.Zone.System.DataStorage.Tests.Query
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Events.Query;
    using EventHorizon.Zone.System.DataStorage.Query;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class QueryForAllDataStoreValuesHandlerTests
    {
        [Fact]
        public async Task ShouldReturnDataFromDataStoreManagementWhenHandled()
        {
            // Given
            var data = new Dictionary<string, object>();
            var expected = new CommandResult<IReadOnlyDictionary<string, object>>(
                data
            );

            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            dataStoreManagementMock.Setup(
                mock => mock.Data()
            ).Returns(
                data
            );

            // When
            var handler = new QueryForAllDataStoreValuesHandler(
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new QueryForAllDataStoreValues(),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
