namespace EventHorizon.Zone.Core.Tests.FileService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ReadAllTextAsBytesFromFileHandlerTests
    {
        [Fact]
        public async Task ShouldReadAllTextAndReturnBytesWhenRequestIsHandled()
        {
            // Given
            var textString = "file-full-text";
            var fileFullName = "file-full-name";
            var expected = textString.ToBytes();

            var fileResolverMock = new Mock<FileResolver>();

            fileResolverMock.Setup(
                mock => mock.GetFileTextAsBytes(
                    fileFullName
                )
            ).Returns(
                expected
            );

            // When
            var handler = new ReadAllTextAsBytesFromFileHandler(
                fileResolverMock.Object
            );
            var actual = await handler.Handle(
                new ReadAllTextAsBytesFromFile(
                    fileFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .BeEquivalentTo(
                    expected
                );
            fileResolverMock.Verify(
                mock => mock.GetFileTextAsBytes(
                    fileFullName
                )
            );
        }
    }
}