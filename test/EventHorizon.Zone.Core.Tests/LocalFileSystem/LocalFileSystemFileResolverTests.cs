namespace EventHorizon.Zone.Core.Tests.LocalFileSystem;

using System;
using System.IO;
using System.Text;

using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Plugin.LocalFileSystem;

using FluentAssertions;

using Xunit;

public class LocalFileSystemFileResolverTests
{
    private readonly LocalFileSystemFileResolver _fileResolver;

    private readonly string _fileTestingDirectory;

    public LocalFileSystemFileResolverTests()
    {
        _fileResolver = new LocalFileSystemFileResolver();
        _fileTestingDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "LocalFileSystem",
            "FileTestingDirectory"
        );
        // Cleanup of the testing workspace.
        if (Directory.Exists(
            _fileTestingDirectory
        ))
        {
            Directory.Delete(
                _fileTestingDirectory,
                true
            );
        }
    }

    [Fact]
    public void TestShouldCreateFileAtPassedInFullNameWhenCreateFileIsCalled()
    {
        // Given
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "FileToBeCreated.txt"
        );

        // When
        File.Exists(
            fileFullName
        ).Should().BeFalse();

        _fileResolver.CreateFile(
            fileFullName
        );

        // Then
        File.Exists(
            fileFullName
        ).Should().BeTrue();
    }

    [Fact]
    public void TestShouldCreateFileAndContainingDirecotryOfFileFullNameWhenCreateFileIsCalled()
    {
        // Given
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "FileToBeCreated.txt"
        );

        // When
        File.Exists(
            fileFullName
        ).Should().BeFalse();
        Directory.Exists(
            _fileTestingDirectory
        ).Should().BeFalse();

        _fileResolver.CreateFile(
            fileFullName
        );

        // Then
        File.Exists(
            fileFullName
        ).Should().BeTrue();
        Directory.Exists(
            _fileTestingDirectory
        ).Should().BeTrue();
    }

    [Fact]
    public void TestShouldGetInfoAboutFileWhenGetFileInfoIsCalled()
    {
        // Given
        var fileName = "SomeRandomFile.txt";
        var directoryFullname = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "SomeRandomDirectory"
        );
        var fileFullName = Path.Combine(
            directoryFullname,
            "SomeRandomFile.txt"
        );
        var fileExtension = ".txt";
        var expected = new StandardFileInfo(
            fileName,
            directoryFullname,
            fileFullName,
            fileExtension
        );

        // When
        var actual = _fileResolver.GetFileInfo(
            fileFullName
        );

        // Then
        actual.Should().Be(
            expected
        );
    }

    [Fact]
    public void TestShouldCreateNewFileWhenTheFileDoesNotExistsForAppending()
    {
        // Given
        var fileText = "this is the text to be added to a new file.";
        var expected = fileText;
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "NotExistingFileForAppend.txt"
        );

        // When
        File.Exists(
            fileFullName
        ).Should().BeFalse();

        _fileResolver.AppendTextToFile(
            fileFullName,
            fileText
        );

        // Then
        File.Exists(
            fileFullName
        ).Should().BeTrue();
        File.ReadAllText(
            fileFullName
        ).Should().Be(
            expected
        );
    }

    [Fact]
    public void TestShouldAppendTextToFileWhenTheFileDoesExistsForAppending()
    {
        // Given
        var fileText = "this is the file text to be appended to an existing file.";
        var expected = fileText;
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "ExistingFileForAppend.txt"
        );

        // When
        var fileInfo = new FileInfo(
            fileFullName
        );
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
        }
        File.WriteAllText(
            fileFullName,
            string.Empty
        );
        File.ReadAllText(
            fileFullName
        ).Should().NotBe(
            expected
        );

        _fileResolver.AppendTextToFile(
            fileFullName,
            fileText
        );

        // Then
        File.Exists(
            fileFullName
        ).Should().BeTrue();
        File.ReadAllText(
            fileFullName
        ).Should().Be(
            expected
        );
    }

    [Fact]
    public void TestShouldDeleteFileWhenExisting()
    {
        // Given
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "ExistingFileToDelete.txt"
        );

        // When
        var fileInfo = new FileInfo(
            fileFullName
        );
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
            File.WriteAllText(
                fileFullName,
                string.Empty
            );
        }

        _fileResolver.DeleteFile(
            fileFullName
        );

        // Then
        new FileInfo(
            fileFullName
        ).Exists.Should().BeFalse();
    }

    [Fact]
    public void TestShouldReadAllTextAsStringWhenDoesExist()
    {
        // Given
        var expected = "this is the expected text.";
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "ExistingFileToDelete.txt"
        );

        // When
        var fileInfo = new FileInfo(
            fileFullName
        );
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
            File.WriteAllText(
                fileFullName,
                expected
            );
        }

        var actual = _fileResolver.GetFileText(
            fileFullName
        );

        // Then
        actual.Should().Be(
            expected
        );
    }

    [Fact]
    public void TestShouldReadAllTextAsBytesWhenDoesExist()
    {
        // Given
        var fileContents = "this is the expected text.";
        var expected = fileContents.ToBytes();
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "ExistingFileToDelete.txt"
        );

        // When
        var fileInfo = new FileInfo(
            fileFullName
        );
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
            File.WriteAllText(
                fileFullName,
                fileContents
            );
        }

        var actual = _fileResolver.GetFileTextAsBytes(
            fileFullName
        );

        // Then
        actual.Should().BeEquivalentTo(
            expected
        );
    }

    [Fact]
    public void TestShouldCreateNewFileWhenTheFileDoesNotExistsWritingAllBytes()
    {
        // Given
        var fileText = "this is the text to be added to a new file.";
        var expected = fileText.ToBytes();
        var fileFullName = Path.Combine(
            _fileTestingDirectory,
            "WritingAllBytes.txt"
        );

        // When
        var fileInfo = new FileInfo(
            fileFullName
        );
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
        }
        fileInfo.Exists.Should().BeFalse();

        _fileResolver.WriteAllBytes(
            fileFullName,
            fileText.ToBytes()
        );

        // Then
        File.Exists(
            fileFullName
        ).Should().BeTrue();
        File.ReadAllBytes(
            fileFullName
        ).Should().BeEquivalentTo(
            expected
        );
    }
}
