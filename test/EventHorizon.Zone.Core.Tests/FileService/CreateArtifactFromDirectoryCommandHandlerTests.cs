﻿namespace EventHorizon.Zone.Core.Tests.FileService;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.FileService;

using FluentAssertions;

using Xunit;

public class CreateArtifactFromDirectoryCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task CreatesZipFileBasedOnDirectoryFullNameWhenGivenValidFileInfo(
        // Given
        CreateArtifactFromDirectoryCommandHandler handler
    )
    {
        GiveDirectoryToBeZipIsSetup(
            nameof(CreatesZipFileBasedOnDirectoryFullNameWhenGivenValidFileInfo),
            out var directoryFullName
        );
        GivenFileZipDoesNotExist(
            nameof(CreatesZipFileBasedOnDirectoryFullNameWhenGivenValidFileInfo),
            out var fileFullName
        );

        // When
        var result = await handler.Handle(
            new CreateArtifactFromDirectoryCommand(
                new Model.DirectoryService.StandardDirectoryInfo(
                    "",
                    directoryFullName,
                    ""
                ),
                fileFullName,
                new List<string>()
            ),
            CancellationToken.None
        );

        // Then
        result.Success.Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public async Task CreatesZipFileWithoutEntryWhenExcludeContainsEntryFullName(
        // Given
        CreateArtifactFromDirectoryCommandHandler handler
    )
    {
        GiveDirectoryToBeZipIsSetup(
            nameof(CreatesZipFileWithoutEntryWhenExcludeContainsEntryFullName),
            out var directoryFullName
        );
        GivenFileZipDoesNotExist(
            nameof(CreatesZipFileWithoutEntryWhenExcludeContainsEntryFullName),
            out var fileFullName
        );

        // When
        var result = await handler.Handle(
            new CreateArtifactFromDirectoryCommand(
                new Model.DirectoryService.StandardDirectoryInfo(
                    "",
                    directoryFullName,
                    ""
                ),
                fileFullName,
                new List<string>
                {
                    "file.txt",
                }
            ),
            CancellationToken.None
        );

        // Then
        result.Success.Should().BeTrue();
        VerifyFileMissingFromArtifact(
            fileFullName,
            "file.txt"
        );
    }

    private static void GiveDirectoryToBeZipIsSetup(
        string directory,
        out string directoryFullName
    )
    {
        // Cleanup any existing directories that might of been created during testing.
        var zipDirectoryParentFullName = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "DirectoryServiceTemp"
        );
        var zipDirectoryFullName = Path.Combine(
            zipDirectoryParentFullName,
            directory
        );
        var directoryFile = Path.Combine(
            zipDirectoryParentFullName,
            directory,
            "file.txt"
        );

        directoryFullName = zipDirectoryFullName;

        // Check for existing directory
        if (!Directory.Exists(
            zipDirectoryFullName
        ))
        {
            Directory.CreateDirectory(
                zipDirectoryFullName
            );
        }

        // Check for Existing Zip file
        if (!File.Exists(directoryFile))
        {
            File.AppendAllText(
                directoryFile,
                "text"
            );
        }
    }

    private static void GivenFileZipDoesNotExist(
        string fileName,
        out string fileFullName
    )
    {
        var zipDirectoryParentFullName = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "DirectoryServiceTemp"
        );
        var zipFileFullName = Path.Combine(
            zipDirectoryParentFullName,
            $"{fileName}.zip"
        );

        fileFullName = zipFileFullName;

        Directory.CreateDirectory(zipDirectoryParentFullName);

        if (File.Exists(
            zipFileFullName
        ))
        {
            File.Delete(
                zipFileFullName
            );
        }
    }

    private static void VerifyFileMissingFromArtifact(
        string artifactFileFullName,
        string excludedEntryFullName
    )
    {

        using var archive = ZipFile.OpenRead(
            artifactFileFullName
        );
        foreach (var entry in archive.Entries)
        {
            if (excludedEntryFullName == entry.FullName)
            {
                throw new Exception(
                    $"'{excludedEntryFullName}' was found in Zip Artifact."
                );
            }
        }
    }
}
