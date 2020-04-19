namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class CreateDirectoryHandlerTests
    {
        [Fact]
        public async Task ShouldCreateDirectoryBasedWithDirectoryFullNameFromRequest()
        {
            // Given
            var expected = true;
            var expectedDirectoryFullname = "directory-full-name";

            var directoryResolverMock = new Mock<DirectoryResolver>();

            directoryResolverMock.Setup(
                mock => mock.CreateDirectory(
                    expectedDirectoryFullname
                )
            ).Returns(
                expected
            );

            // When
            var handler = new CreateDirectoryHandler(
                directoryResolverMock.Object
            );
            var actual = await handler.Handle(
                new Events.DirectoryService.CreateDirectory(
                    expectedDirectoryFullname
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
            directoryResolverMock.Verify(
                mock => mock.CreateDirectory(
                    expectedDirectoryFullname
                )
            );
        }
    }
}