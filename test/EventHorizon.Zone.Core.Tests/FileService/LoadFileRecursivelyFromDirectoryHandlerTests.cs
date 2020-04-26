using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.FileService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;
using Moq;
using Xunit;
using FluentAssertions;

namespace EventHorizon.Zone.Core.Tests.FileService
{
    public class ProcessFilesRecursivelyFromDirectoryHandlerTests
    {
        private struct ProcessedFileInfo
        {
            public StandardFileInfo FileInfo { get; set; }
            public IDictionary<string, object> Args { get; set; }
        }

        [Fact]
        public async Task ShouldProcessExpectedFilesAndDirectoriesBasedOnRequestParameters()
        {
            // Given
            var actualProcessedFileInfo = new List<ProcessedFileInfo>();

            var fromDirectoryFullName = "from-directory-full-name";
            var arguments = new Dictionary<string, object>();

            var fromDirectoryFiles = new List<StandardFileInfo>();
            var fromDirectoryFileName = "from-directory-file-name";
            var fromDirectoryFileFullName = "from-directory-file-full-name";
            var fromDirectoryFileExtension = "from-directory-file-extension";
            var fromDirectoryFile = new StandardFileInfo(
                fromDirectoryFileName,
                fromDirectoryFullName,
                fromDirectoryFileFullName,
                fromDirectoryFileExtension
            );
            fromDirectoryFiles.Add(
                fromDirectoryFile
            );

            var subDirectories = new List<StandardDirectoryInfo>();
            var subDirectoryName = "sub-directory-name";
            var subDirectoryFullName = "sub-directory-full-name";
            var subDirectory = new StandardDirectoryInfo(
                subDirectoryName,
                subDirectoryFullName,
                fromDirectoryFullName
            );
            subDirectories.Add(
                subDirectory
            );
            var subDirectoryFiles = new List<StandardFileInfo>();
            var subDirectoryFileName = "sub-directory-file-name";
            var subDirectoryFileFullName = "sub-directory-file-full-name";
            var subDirectoryFileExtension = "sub-directory-file-extension";
            var subDirectoryFile = new StandardFileInfo(
                subDirectoryFileName,
                subDirectoryFullName,
                subDirectoryFileFullName,
                subDirectoryFileExtension
            );
            subDirectoryFiles.Add(
                subDirectoryFile
            );

            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile =
                (fileInfo, args) =>
                {
                    actualProcessedFileInfo.Add(
                        new ProcessedFileInfo
                        {
                            FileInfo = fileInfo,
                            Args = args,
                        }
                    );
                    return Task.CompletedTask;
                };

            var mediatorMock = new Mock<IMediator>();

            // Setup the SubDirectory List for FromDirectory
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfDirectoriesFromDirectory(
                        fromDirectoryFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                subDirectories
            );

            // Setup the File List for SubDirectory
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        subDirectoryFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                subDirectoryFiles
            );

            // Setup the File List for FromDirectory
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        fromDirectoryFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fromDirectoryFiles
            );

            // When
            var handler = new ProcessFilesRecursivelyFromDirectoryHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ProcessFilesRecursivelyFromDirectory(
                    fromDirectoryFullName,
                    onProcessFile,
                    arguments
                ),
                CancellationToken.None
            );

            // Then
            actualProcessedFileInfo.Select(c => c.FileInfo).Should()
                .NotBeEmpty()
                .And.HaveCount(2)
                .And.Equal(
                    subDirectoryFile,
                    fromDirectoryFile
                );
            actualProcessedFileInfo.Select(c => c.Args).Should()
                .NotBeEmpty()
                .And.HaveCount(2)
                .And.Equal(
                    arguments,
                    arguments
                );
        }
    }
}