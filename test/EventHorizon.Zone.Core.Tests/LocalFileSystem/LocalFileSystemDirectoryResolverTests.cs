namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    using System;
    using System.IO;
    using System.Linq;

    using EventHorizon.Zone.Core.Model.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Plugin.LocalFileSystem;

    using FluentAssertions;

    using Xunit;

    public class LocalFileSystemDirectoryResolverTests
    {
        string notExistingDirectoryName = "";
        string notExistingDirectoryFullName = "";
        string notExistingDirectoryParentFullName = "";

        public LocalFileSystemDirectoryResolverTests()
        {
            // Cleanup any existing directories that might of been created during testing.
            notExistingDirectoryName = "DirectoryToBeCreated";
            notExistingDirectoryParentFullName = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem"
            );
            notExistingDirectoryFullName = Path.Combine(
                notExistingDirectoryParentFullName,
                notExistingDirectoryName
            );

            if (Directory.Exists(
                notExistingDirectoryFullName
            ))
            {
                Directory.Delete(
                    notExistingDirectoryFullName
                );
            }
        }

        [Fact]
        public void TestShouldCreateDirectoryAtPassedInFullNameWhenCreateDirectoryIsCalled()
        {
            // Given
            var input = notExistingDirectoryFullName;
            var expected = true;

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.CreateDirectory(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldDeleteDirectoryAtPassedInFullNameWhenDeleteDirectoryIsCalled()
        {
            // Given
            var input = notExistingDirectoryFullName;
            var expected = false;

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            resolver.CreateDirectory(
                input
            ).Should().Be(
                true
            );
            resolver.DeleteDirectory(
                input
            );
            var actual = resolver.DoesDirectoryExist(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnListOfDirectoriesWhenGetDirectoriesIsCalled()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest"
            );
            var directory1Name = "TestDirectory1";
            var directory1 = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest",
                "TestDirectory1"
            );
            var directory1ParentFullName = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest"
            );
            var expectedDirectory1 = new StandardDirectoryInfo(
                directory1Name,
                directory1,
                directory1ParentFullName
            );
            var directory2Name = "TestDirectory2";
            var directory2 = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest",
                "TestDirectory2"
            );
            var directory2ParentFullName = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest"
            );
            var expectedDirectory2 = new StandardDirectoryInfo(
                directory2Name,
                directory2,
                directory2ParentFullName
            );

            // When
            var directoryResolver = new LocalFileSystemDirectoryResolver();
            var actual = directoryResolver.GetDirectories(
                path
            ).OrderBy(
                directory => directory.Name
            );

            // Then
            Assert.Collection(
                actual,
                directory => directory.Should().Be(expectedDirectory1),
                directory => directory.Should().Be(expectedDirectory2)
            );
        }

        [Fact]
        public void TestShouldReturnListOfFilesWhenGetFilesIsCalled()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest"
            );
            var file1Name = ".gitkeep";
            var file1DirectoryName = path;
            var file1FullName = Path.Combine(
                file1DirectoryName,
                ".gitkeep"
            );
            var file1Extension = ".gitkeep";
            var expectedFile1 = new StandardFileInfo(
                file1Name,
                file1DirectoryName,
                file1FullName,
                file1Extension
            );
            var file2Name = "test-file-1.txt";
            var file2DirectoryName = path;
            var file2FullName = Path.Combine(
                file2DirectoryName,
                "test-file-1.txt"
            );
            var file2Extension = ".txt";
            var expectedFile2 = new StandardFileInfo(
                file2Name,
                file2DirectoryName,
                file2FullName,
                file2Extension
            );

            // When
            var directoryResolver = new LocalFileSystemDirectoryResolver();
            var actual = directoryResolver.GetFiles(
                path
            ).OrderBy(
                file => file.Name
            );

            // Then 
            Assert.Collection(
                actual,
                file => Assert.Equal(expectedFile1, file),
                file => Assert.Equal(expectedFile2, file)
            );
        }

        [Fact]
        public void TestShouldGetDirectoryInfoWhenDirectoryDoesExist()
        {
            // Given
            var testPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem"
            );
            var directoryName = "DirectoryToTest";
            var directoryFullName = Path.Combine(
                testPath,
                "DirectoryToTest"
            );
            var parentDirectoryFullname = testPath;

            var expected = new StandardDirectoryInfo(
                directoryName,
                directoryFullName,
                parentDirectoryFullname
            );

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.GetDirectoryInfo(
                directoryFullName
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnDirectoryInfoWhenDirectoryDoesNotExist()
        {
            // Given
            var directoryName = notExistingDirectoryName;
            var directoryFullName = notExistingDirectoryFullName;
            var directoryParentFullname = notExistingDirectoryParentFullName;

            var expected = new StandardDirectoryInfo(
                directoryName,
                directoryFullName,
                directoryParentFullname
            );

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.GetDirectoryInfo(
                directoryFullName
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnFalseWhenDirectoryContainsFiles()
        {
            // Given
            var expected = false;
            var input = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest"
            );

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.IsEmpty(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnFalseWhenDirectoryContainsDirectories()
        {
            // Given
            var expected = false;
            var input = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryToTest"
            );

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.IsEmpty(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnFalseWhenDirectoryOnlyContainsFiles()
        {
            // Given
            var expected = false;
            var input = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryOnlyFilesToTest"
            );
            var fileToBeCreated = Path.Combine(
                input,
                "FileToBeCreated.txt"
            );
            Directory.CreateDirectory(
                input
            );
            File.WriteAllText(
                fileToBeCreated,
                "text"
            );

            // When
            new DirectoryInfo(
                input
            ).GetFiles().Should().HaveCountGreaterOrEqualTo(1);
            new DirectoryInfo(
                input
            ).GetDirectories().Should().BeEmpty();
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.IsEmpty(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnFalseWhenDirectoryContainsOnlyDirectories()
        {
            // Given
            var expected = false;
            var input = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LocalFileSystem",
                "DirectoryOnlyDirectoriesToTest",
                "DirectoryToBeCreated"
            );
            var directoryToBeCreated = Path.Combine(
                input,
                "DirectoryToBeCreated"
            );
            Directory.CreateDirectory(
                directoryToBeCreated
            );

            // When
            new DirectoryInfo(
                input
            ).GetFiles().Should().BeEmpty();
            new DirectoryInfo(
                input
            ).GetDirectories().Should().HaveCountGreaterOrEqualTo(1);
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.IsEmpty(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnTrueWhenDirectoryDoesNotExist()
        {
            // Given
            var expected = true;
            var input = notExistingDirectoryFullName;

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.IsEmpty(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }

        [Fact]
        public void TestShouldReturnTrueWhenDirectoryExistsAndDoesNotContainDirectoriesAndFiles()
        {
            // Given
            var expected = true;
            var input = notExistingDirectoryFullName;
            // Make sure the input directory exists, create is not
            if (!Directory.Exists(
                input
            ))
            {
                Directory.CreateDirectory(
                    input
                );
            }

            // When
            var resolver = new LocalFileSystemDirectoryResolver();
            var actual = resolver.IsEmpty(
                input
            );

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
