using EventHorizon.Zone.System.Watcher.Model;
using EventHorizon.Zone.System.Watcher.State;

using Moq;

using Xunit;

namespace EventHorizon.Zone.System.Watcher.Tests.State
{
    public class InMemoryFileSystemWatcherStateTests
    {
        [Fact]
        public void TestShouldAddPathWatcherByPath()
        {
            // Given
            var path = "path-to-watched";
            var pathWatcherMock = new Mock<PathWatcher>();
            var expected = pathWatcherMock.Object;

            // When
            var fileSystemWatcher = new InMemoryFileSystemWatcherState();

            fileSystemWatcher.Add(
                path,
                pathWatcherMock.Object
            );

            var actual = fileSystemWatcher.Get(
                path
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldRemoveFromStateByPathWhenRemoveBySamePath()
        {
            // Given
            var path = "path-to-watched";
            var pathWatcherMock = new StandardPathWatcher(
                path,
                null
            );
            var expected = pathWatcherMock;

            // When
            var fileSystemWatcher = new InMemoryFileSystemWatcherState();

            fileSystemWatcher.Add(
                path,
                pathWatcherMock
            );

            var existing = fileSystemWatcher.Get(
                path
            );
            // Verify it is in State
            Assert.Equal(
                expected,
                existing
            );

            fileSystemWatcher.Remove(
                path
            );
            var actual = fileSystemWatcher.Get(
                path
            );

            // Then
            Assert.NotEqual(
                expected,
                actual
            );
        }
    }
}
