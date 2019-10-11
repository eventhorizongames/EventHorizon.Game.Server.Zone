using System.IO;
using EventHorizon.Zone.System.Watcher.Model;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Watcher.Tests.Model
{
    public class StandardPathWatcherTests
    {
        [Fact]
        public void TestShouldAbstractThePathAndFileSystemWatcherWhenCreatedWithStandardPathWatcher()
        {
            // Given
            var expected = "path-to-a-file-or-directory";
            var fileSystemWatcherMock = new Mock<FileSystemWatcher>();

            // When
            var standardPatchWatcher = new StandardPathWatcher(
                expected,
                fileSystemWatcherMock.Object
            );

            standardPatchWatcher.Dispose();

            // Then
            Assert.Equal(
                expected,
                standardPatchWatcher.Path
            );
        }
    }
}