namespace EventHorizon.Zone.System.DataStorage.Tests.Load
{
    using AutoFixture.Xunit2;
    using EventHorizon.Test.Common.Attributes;
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
        [Theory, AutoMoqData]
        public async Task ShouldSetDataStoreLoadedFromFileLoaderWhenNotNull(
            // Given
            Dictionary<string, object> dataStoreFile,
            [Frozen] Mock<IJsonFileLoader> fileLoaderMock,
            [Frozen] Mock<DataStoreManagement> dataStoreManagementMock,
            LoadDataStoreCommandHandler handler
        )
        {
            fileLoaderMock.Setup(
                mock => mock.GetFile<Dictionary<string, object>>(
                    It.IsAny<string>()
                )
            ).ReturnsAsync(
                dataStoreFile
            );

            // When
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

        [Theory, AutoMoqData]
        public async Task ShouldNotSetDataStoreWhenFileLoaderReturnsNullDictionary(
            // Given
            [Frozen] Mock<IJsonFileLoader> fileLoaderMock,
            [Frozen] Mock<DataStoreManagement> dataStoreManagementMock,
            LoadDataStoreCommandHandler handler
        )
        {
            fileLoaderMock.Setup(
                mock => mock.GetFile<Dictionary<string, object>>(
                    It.IsAny<string>()
                )
            ).ReturnsAsync(
                default(Dictionary<string, object>)
            );

            // When
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
