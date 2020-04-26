namespace EventHorizon.Zone.Core.Tests.FileService
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using Moq;
    using Xunit;

    public class AppendTextToFileHandlerTests
    {
        [Fact]
        public async Task ShouldCallAppendToFileWithRequestPropertiesWhenRequestIsHandled()
        {
            // Given
            var expectedFileFullName = "file-full-name";
            var expectedText = "file-text";

            var fileResolverMock = new Mock<FileResolver>();

            // When
            var handler = new AppendTextToFileHandler(
                fileResolverMock.Object
            );
            await handler.Handle(
                new Events.FileService.AppendTextToFile(
                    expectedFileFullName,
                    expectedText
                ),
                CancellationToken.None
            );

            // Then
            fileResolverMock.Verify(
                mock => mock.AppendTextToFile(
                    expectedFileFullName,
                    expectedText
                )
            );
        }
    }
}