namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.DirectoryService;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class DeleteDirectoryHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenDirectoryIsEmpty()
        {
            // Given
            var directorFullName = "directory-full-name";
            var expected = true;

            var directoryResolverMock = new Mock<DirectoryResolver>();

            directoryResolverMock.Setup(
                mock => mock.IsEmpty(
                    directorFullName
                )
            ).Returns(
                true
            );

            // When
            var handler = new DeleteDirectoryHandler(
                directoryResolverMock.Object
            );
            var actual = await handler.Handle(
                new DeleteDirectory(
                    directorFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public async Task ShouldReturnFalseWhenDirectoryIsNotEmpty()
        {
            // Given
            var directorFullName = "directory-full-name";
            var expected = false;

            var directoryResolverMock = new Mock<DirectoryResolver>();

            directoryResolverMock.Setup(
                mock => mock.IsEmpty(
                    directorFullName
                )
            ).Returns(
                false
            );

            // When
            var handler = new DeleteDirectoryHandler(
                directoryResolverMock.Object
            );
            var actual = await handler.Handle(
                new DeleteDirectory(
                    directorFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
