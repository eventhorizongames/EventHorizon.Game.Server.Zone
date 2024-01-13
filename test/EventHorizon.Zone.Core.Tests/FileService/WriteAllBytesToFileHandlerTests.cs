namespace EventHorizon.Zone.Core.Tests.FileService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using Moq;
    using Xunit;

    public class WriteAllBytesToFileHandlerTests
    {
        [Fact]
        public async Task ShouldReadAllTextAndReturnBytesWhenRequestIsHandled()
        {
            // Given
            var fileFullName = "file-full-name";
            var fileText = "file-full-text";
            var expected = fileText.ToBytes();

            var fileResolverMock = new Mock<FileResolver>();

            // When
            var handler = new WriteAllBytesToFileHandler(fileResolverMock.Object);
            await handler.Handle(
                new WriteAllBytesToFile(fileFullName, expected),
                CancellationToken.None
            );

            // Then
            fileResolverMock.Verify(mock => mock.WriteAllBytes(fileFullName, expected));
        }
    }
}
