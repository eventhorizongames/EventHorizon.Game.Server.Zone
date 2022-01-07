namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Query;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System;

using global::System.Collections.Generic;
using global::System.Threading.Tasks;
using global::System.Threading;
using Moq;
using Xunit;
using EventHorizon.Zone.System.ArtifactManagement.Query;
using MediatR;
using FluentAssertions;
using global::System.IO;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

public class QueryForExcludedArtifactEntriesHandlerTests
{
    [Theory, AutoMoqData]
    public async Task Return(
        // Given
        [Frozen] Mock<ISender> senderMock,
        QueryForExcludedArtifactEntriesHandler handler
    )
    {
        var directoryFullName = Path.Combine(
            $"{Path.DirectorySeparatorChar}path",
            "to",
            "a",
            "directory"
        );
        senderMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    directoryFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<StandardFileInfo>
            {
                new StandardFileInfo(
                    "File1.json",
                    directoryFullName,
                    Path.Combine(
                        directoryFullName,
                        "File1.json"
                    ),
                    ".json"
                ),
                new StandardFileInfo(
                    "File2.json",
                    directoryFullName,
                    Path.Combine(
                        directoryFullName,
                        "File2.json"
                    ),
                    ".json"
                ),
                new StandardFileInfo(
                    "File3.json",
                    directoryFullName,
                    Path.Combine(
                        directoryFullName,
                        "SubDirectory",
                        "File3.json"
                    ),
                    ".json"
                ),
            }
        );

        // When
        var actual = await handler.Handle(
            new QueryForExcludedArtifactEntries(
                directoryFullName
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().BeEquivalentTo(
            "File1.json",
            "File2.json",
            Path.Combine(
                "SubDirectory",
                "File3.json"
            )
        );
    }
}
