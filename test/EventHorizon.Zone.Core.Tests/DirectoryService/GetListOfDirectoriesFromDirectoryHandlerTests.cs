namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.DirectoryService;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class GetListOfDirectoriesFromDirectoryHandlerTests
    {
        [Fact]
        public async Task ShouldReturnStandardDirectoryInfoForDirectorFullNameInRequest()
        {
            // Given
            var directoryName = "directory-name";
            var directoryFullName = "directory-full-name";
            var parentFullName = "parent-full-name";
            var expected = new[]{
                new StandardDirectoryInfo(
                    directoryName,
                    directoryFullName,
                    parentFullName
                )
            }.ToList();

            var directoryResolverMock = new Mock<DirectoryResolver>();

            directoryResolverMock.Setup(
                mock => mock.GetDirectories(
                    directoryFullName
                )
            ).Returns(
                expected
            );

            // When
            var handler = new GetListOfDirectoriesFromDirectoryHandler(
                directoryResolverMock.Object
            );
            var actual = await handler.Handle(
                new GetListOfDirectoriesFromDirectory(
                    directoryFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
            directoryResolverMock.Verify(
                mock => mock.GetDirectories(
                    directoryFullName
                )
            );
        }
    }
}
