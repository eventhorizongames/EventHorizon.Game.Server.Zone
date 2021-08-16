namespace EventHorizon.Zone.System.DataStorage.Tests.Save
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Save;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class SaveDataStoreCommandHandlerTests
    {
        [Fact]
        public async Task ShouldSaveDataStoreDataUsingFileSaverWhenCommandIsHandled()
        {
            // Given
            var appDataPath = "app-data-path";
            var saveDirectory = Path.Combine(
                appDataPath,
                "DataStorage"
            );
            var saveFile = Path.Combine(
                saveDirectory,
                "DataStore.json"
            );
            var data = new Dictionary<string, object>();

            var serverInfoMock = new Mock<ServerInfo>();
            var fileSaverMock = new Mock<IJsonFileSaver>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            dataStoreManagementMock.Setup(
                mock => mock.Data()
            ).Returns(
                data
            );

            // When
            var handler = new SaveDataStoreCommandHandler(
                serverInfoMock.Object,
                fileSaverMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new SaveDataStoreCommand(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();

            fileSaverMock.Verify(
                mock => mock.SaveToFile(
                    saveDirectory,
                    saveFile,
                    data
                )
            );
        }
    }
}
