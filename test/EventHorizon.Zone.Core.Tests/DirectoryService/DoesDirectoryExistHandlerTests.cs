using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;

using FluentAssertions;

using Moq;

using Xunit;

namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    public class DoesDirectoryExistHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenDirectorFullNameInRequest()
        {
            // Given
            var directoryFullName = "directory-full-name";
            var expected = true;

            var directoryResolverMock = new Mock<DirectoryResolver>();

            directoryResolverMock.Setup(
                mock => mock.DoesDirectoryExist(
                    directoryFullName
                )
            ).Returns(
                true
            );

            // When
            var handler = new DoesDirectoryExistHandler(
                directoryResolverMock.Object
            );
            var actual = await handler.Handle(
                new Events.DirectoryService.DoesDirectoryExist(
                    directoryFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
            directoryResolverMock.Verify(
                mock => mock.DoesDirectoryExist(
                    directoryFullName
                )
            );
        }
    }
}
