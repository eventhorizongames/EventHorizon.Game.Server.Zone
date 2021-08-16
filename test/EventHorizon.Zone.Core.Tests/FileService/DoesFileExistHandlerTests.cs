namespace EventHorizon.Zone.Core.Tests.FileService
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.FileService;
    using EventHorizon.Zone.Core.Model.FileService;

    using Moq;

    using Xunit;

    public class DoesFileExistHandlerTests
    {
        [Fact]
        public async Task ShouldCallDeleteFileWithRequestPropertiesWhenRequestIsHandled()
        {
            // Given
            var expected = "file-full-name";

            var fileResolverMock = new Mock<FileResolver>();

            // When
            var handler = new DoesFileExistHandler(
                fileResolverMock.Object
            );
            await handler.Handle(
                new DoesFileExist(
                    expected
                ),
                CancellationToken.None
            );

            // Then
            fileResolverMock.Verify(
                mock => mock.DoesFileExist(
                    expected
                )
            );
        }
    }
}
