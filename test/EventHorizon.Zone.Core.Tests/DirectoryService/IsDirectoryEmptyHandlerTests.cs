namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.DirectoryService;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class IsDirectoryEmptyHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenDirectorFullNameInRequest()
        {
            // Given
            var directoryFullName = "directory-full-name";
            var expected = true;

            var directoryResolverMock = new Mock<DirectoryResolver>();

            directoryResolverMock.Setup(
                mock => mock.IsEmpty(
                    directoryFullName
                )
            ).Returns(
                true
            );

            // When
            var handler = new IsDirectoryEmptyHandler(
                directoryResolverMock.Object
            );
            var actual = await handler.Handle(
                new IsDirectoryEmpty(
                    directoryFullName
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
            directoryResolverMock.Verify(
                mock => mock.IsEmpty(
                    directoryFullName
                )
            );
        }
    }
}
