namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.DirectoryService;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using Moq;
    using Xunit;
    using FluentAssertions;

    public class GetDirectoryInfoHandlerTests
    {
        [Fact]
        public async Task ShouldReturnDirectoryInfoForDirectorFullNameInRequest()
        {
            // Given
            var directoryName = "directory-name";
            var directoryFullName = "directory-full-name";
            var parentFullName = "parent-full-name";
            var expected = new StandardDirectoryInfo(
                directoryName,
                directoryFullName,
                parentFullName
            );

            var directoryResolverMock = new Mock<DirectoryResolver>();

            directoryResolverMock.Setup(
                mock => mock.GetDirectoryInfo(
                    directoryFullName
                )
            ).Returns(
                expected
            );

            // When
            var handler = new GetDirectoryInfoHandler(
                directoryResolverMock.Object
            );
            var actual = await handler.Handle(
                new GetDirectoryInfo(
                    directoryFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
            directoryResolverMock.Verify(
                mock => mock.GetDirectoryInfo(
                    directoryFullName
                )
            );
        }
    }
}