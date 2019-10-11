using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Watcher.Events.Start;
using EventHorizon.Zone.System.Watcher.Model;
using EventHorizon.Zone.System.Watcher.Start;
using EventHorizon.Zone.System.Watcher.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Watcher.Tests.Start
{
    public class StartWatchingFileSystemCommandHandlerTests
    {

        [Fact]
        public async Task TestShouldDisposeOfExistingPathWatcherWhenContainedInState()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Start",
                "FolderToWatch"
            );
            var existingPathWatcherMock = new Mock<PathWatcher>();

            var reloadStateMock = new Mock<PendingReloadState>();
            var fileSystemWatcherStateMock = new Mock<FileSystemWatcherState>();

            fileSystemWatcherStateMock.Setup(
                mock => mock.Get(
                    path
                )
            ).Returns(
                existingPathWatcherMock.Object
            );

            // When
            var handler = new StartWatchingFileSystemCommandHandler(
                reloadStateMock.Object,
                fileSystemWatcherStateMock.Object
            );
            await handler.Handle(
                new StartWatchingFileSystemCommand(
                    path
                ),
                CancellationToken.None
            );

            // Then
            existingPathWatcherMock.Verify(
                mock => mock.Dispose()
            );
        }

        [Fact]
        public async Task TestShouldRemovePathWatcherFromState()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Start",
                "FolderToWatch"
            );
            var existingPathWatcherMock = new Mock<PathWatcher>();

            var reloadStateMock = new Mock<PendingReloadState>();
            var fileSystemWatcherStateMock = new Mock<FileSystemWatcherState>();

            fileSystemWatcherStateMock.Setup(
                mock => mock.Get(
                    path
                )
            ).Returns(
                existingPathWatcherMock.Object
            );

            // When
            var handler = new StartWatchingFileSystemCommandHandler(
                reloadStateMock.Object,
                fileSystemWatcherStateMock.Object
            );
            await handler.Handle(
                new StartWatchingFileSystemCommand(
                    path
                ),
                CancellationToken.None
            );

            // Then
            fileSystemWatcherStateMock.Setup(
                mock => mock.Remove(
                    path
                )
            );
        }

        [Fact]
        public async Task TestShouldAddStandardPathWatcherWhenCalledWithValidPath()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Start",
                "FolderToWatch"
            );
            var existingPathWatcherMock = new Mock<PathWatcher>();

            var reloadStateMock = new Mock<PendingReloadState>();
            var fileSystemWatcherStateMock = new Mock<FileSystemWatcherState>();

            fileSystemWatcherStateMock.Setup(
                mock => mock.Get(
                    path
                )
            ).Returns(
                existingPathWatcherMock.Object
            );

            // When
            var handler = new StartWatchingFileSystemCommandHandler(
                reloadStateMock.Object,
                fileSystemWatcherStateMock.Object
            );
            await handler.Handle(
                new StartWatchingFileSystemCommand(
                    path
                ),
                CancellationToken.None
            );

            // Then
            fileSystemWatcherStateMock.Setup(
                mock => mock.Add(
                    path,
                    It.IsAny<StandardPathWatcher>()
                )
            );
        }

        [Fact]
        public async Task TestShouldCreateEventForOnChangeCallbacksWhenFileSystemChangesCreatedOrDeleted()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Start",
                "FolderToWatch"
            );
            var jsonTestFile = Path.Combine(
                path,
                "TestFile.json"
            );
            var existingPathWatcherMock = new Mock<PathWatcher>();
            // Cleanup any existing files, file might exist on failed run.
            File.Delete(
                jsonTestFile
            );

            var reloadStateMock = new Mock<PendingReloadState>();
            var fileSystemWatcherStateMock = new Mock<FileSystemWatcherState>();

            fileSystemWatcherStateMock.Setup(
                mock => mock.Get(
                    path
                )
            ).Returns(
                existingPathWatcherMock.Object
            );

            // When
            var handler = new StartWatchingFileSystemCommandHandler(
                reloadStateMock.Object,
                fileSystemWatcherStateMock.Object
            );
            await handler.Handle(
                new StartWatchingFileSystemCommand(
                    path
                ),
                CancellationToken.None
            );
            // Create File (1 - Created, 2 - Access)
            File.WriteAllText(
                jsonTestFile,
                "This is test text"
            );
            // Change File (3 - Write, 4 - Access)
            File.WriteAllText(
                jsonTestFile,
                "This Is new Text"
            );
            // Delete File (5 - Deleted)
            File.Delete(
                jsonTestFile
            );

            // Then
            reloadStateMock.Verify(
                mock => mock.SetToPending(),
                Times.Exactly(5)
            );
        }
        
        [Fact]
        public async Task TestShouldNotCauseExceptionWhenAPathWatcherIsNotExisting()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Start",
                "FolderToWatch"
            );

            var reloadStateMock = new Mock<PendingReloadState>();
            var fileSystemWatcherStateMock = new Mock<FileSystemWatcherState>();

            // When
            var handler = new StartWatchingFileSystemCommandHandler(
                reloadStateMock.Object,
                fileSystemWatcherStateMock.Object
            );
            await handler.Handle(
                new StartWatchingFileSystemCommand(
                    path
                ),
                CancellationToken.None
            );

            // Then
        }
    }
}