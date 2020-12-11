namespace EventHorizon.Zone.System.Watcher.Tests.Start
{
    using global::System;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Watcher.Events.Start;
    using EventHorizon.Zone.System.Watcher.Model;
    using EventHorizon.Zone.System.Watcher.Start;
    using EventHorizon.Zone.System.Watcher.State;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

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

            var loggerMock = new Mock<ILogger<StartWatchingFileSystemCommandHandler>>();
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
                loggerMock.Object,
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

            var loggerMock = new Mock<ILogger<StartWatchingFileSystemCommandHandler>>();
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
                loggerMock.Object,
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

            var loggerMock = new Mock<ILogger<StartWatchingFileSystemCommandHandler>>();
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
                loggerMock.Object,
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
        [Trait("WindowsOnly", "True")]
        // This test will only work on Linux when File System Pooling is enabled.
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

            var loggerMock = new Mock<ILogger<StartWatchingFileSystemCommandHandler>>();
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
                loggerMock.Object,
                reloadStateMock.Object,
                fileSystemWatcherStateMock.Object
            );
            await handler.Handle(
                new StartWatchingFileSystemCommand(
                    path
                ),
                CancellationToken.None
            );
            // Create File (1 - Created)
            File.WriteAllText(
                jsonTestFile,
                "This is test text"
            );
            // Change File (2 - Write)
            File.WriteAllText(
                jsonTestFile,
                "This Is new Text"
            );
            // Delete File (3 - Deleted)
            File.Delete(
                jsonTestFile
            );

            // Then
            reloadStateMock.Verify(
                mock => mock.SetToPending(),
                // Can be Called greater than 3 times based on env used for testing
                // But should be at least three for the main types of actions done.
                Times.AtLeast(3)
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

            var loggerMock = new Mock<ILogger<StartWatchingFileSystemCommandHandler>>();
            var reloadStateMock = new Mock<PendingReloadState>();
            var fileSystemWatcherStateMock = new Mock<FileSystemWatcherState>();

            // When
            var handler = new StartWatchingFileSystemCommandHandler(
                loggerMock.Object,
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