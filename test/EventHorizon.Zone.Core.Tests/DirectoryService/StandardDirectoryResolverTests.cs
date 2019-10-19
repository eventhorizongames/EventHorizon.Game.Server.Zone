using System;
using System.Linq;
using System.IO;
using EventHorizon.Zone.Core.DirectoryService;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.DirectoryService
{
    public class StandardDirectoryResolverTests
    {
        [Fact]
        public void TestShouldReturnListOfDirectoriesWhenGetDirectoriesIsCalled()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "DirectoryService",
                "DirectoryToTest"
            );
            var expectedDirectory1 = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "DirectoryService",
                "DirectoryToTest",
                "TestDirectory1"
            );
            var expectedDirectory2 = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "DirectoryService",
                "DirectoryToTest",
                "TestDirectory2"
            );

            // When
            var directoryResolver = new StandardDirectoryResolver();
            var actual = directoryResolver.GetDirectories(
                path
            ).OrderBy(
                directory => directory
            );

            // Then
            Assert.Collection(
                actual,
                directory => Assert.Equal(expectedDirectory1, directory),
                directory => Assert.Equal(expectedDirectory2, directory)
            );
        }

        [Fact]
        public void TestShouldReturnListOfFilesWhenGetFilesIsCalled()
        {
            // Given
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "DirectoryService",
                "DirectoryToTest"
            );
            var expectedFile1 = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "DirectoryService",
                "DirectoryToTest",
                ".gitkeep"
            );
            var expectedFile2 = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "DirectoryService",
                "DirectoryToTest",
                "test-file-1.txt"
            );

            // When
            var directoryResolver = new StandardDirectoryResolver();
            var actual = directoryResolver.GetFiles(
                path
            ).OrderBy(
                file => file
            );

            // Then 
            Assert.Collection(
                actual,
                file => Assert.Equal(expectedFile1, file),
                file => Assert.Equal(expectedFile2, file)
            );
        }
    }
}