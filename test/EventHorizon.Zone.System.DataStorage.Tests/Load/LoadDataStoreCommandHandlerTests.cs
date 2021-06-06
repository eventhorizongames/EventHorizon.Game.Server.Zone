namespace EventHorizon.Zone.System.DataStorage.Tests.Load
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Load;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class LoadDataStoreCommandHandlerTests
    {
        [Fact]
        public async Task ShouldSetDataStoreLoadedFromFileLoaderWhenNotNull()
        {
            // Given
            var appDataPath = "app-data-path";
            var dataStoreFile = new Dictionary<string, object>();

            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<Dictionary<string, object>>(
                    It.IsAny<string>()
                )
            ).ReturnsAsync(
                dataStoreFile
            );

            // When
            var handler = new LoadDataStoreCommandHandler(
                serverInfoMock.Object,
                fileLoaderMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new LoadDataStoreCommand(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();

            dataStoreManagementMock.Verify(
                mock => mock.Set(
                    dataStoreFile
                )
            );
        }

        [Fact]
        public async Task ShouldNotSetDataStoreWhenFileLoaderReturnsNullDictionary()
        {
            // Given
            var appDataPath = "app-data-path";
            var dataStoreFile = default(Dictionary<string, object>);

            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<Dictionary<string, object>>(
                    It.IsAny<string>()
                )
            ).ReturnsAsync(
                dataStoreFile
            );

            // When
            var handler = new LoadDataStoreCommandHandler(
                serverInfoMock.Object,
                fileLoaderMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new LoadDataStoreCommand(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();

            dataStoreManagementMock.Verify(
                mock => mock.Set(
                    It.IsAny<Dictionary<string, object>>()
                ),
                Times.Never()
            );
        }
    }
}
