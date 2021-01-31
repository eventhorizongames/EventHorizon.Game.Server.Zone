namespace EventHorizon.Zone.Core.Tests.Info
{
    using System;
    using System.IO;
    using EventHorizon.Zone.Core.Info;
    using Microsoft.Extensions.Hosting;
    using Moq;
    using Xunit;

    public class HostingEnvironmentServerInfoTests
    {
        [Fact]
        public void TestShouldHavePathRelativePathWhenGeneatedFromHostingEnvionrment()
        {
            // Given
            var contentRootPath = "path";

            var expectedRootPath = contentRootPath;
            var expectedFileSystemTempPath = ToValidPath(Path.DirectorySeparatorChar.ToString(), "temp");
            var expectedAssembliesPathPath = AppDomain.CurrentDomain.BaseDirectory;
            var expectedGeneratedPath = ToValidPath(contentRootPath, "App_Data", "_generated");
            var expectedAppDataPath = ToValidPath(contentRootPath, "App_Data");
            var expectedSystemPath = ToValidPath(contentRootPath, "App_Data", "System");
            var expectedSystemBackupPath = ToValidPath(contentRootPath, "App_Data", "__Backup__");
            var expectedAdminPath = ToValidPath(contentRootPath, "App_Data", "Admin");
            var expectedPluginsPath = ToValidPath(contentRootPath, "App_Data", "Plugins");
            var expectedI18nPath = ToValidPath(contentRootPath, "App_Data", "I18n");
            var expectedClientPath = ToValidPath(contentRootPath, "App_Data", "Client");
            var expectedClientScriptsPath = ToValidPath(contentRootPath, "App_Data", "Client", "Scripts");
            var expectedClientEntityPath = ToValidPath(contentRootPath, "App_Data", "Client", "Entity");
            var expectedServerPath = ToValidPath(contentRootPath, "App_Data", "Server");
            var expectedServerScriptsPath = ToValidPath(contentRootPath, "App_Data", "Server", "Scripts");
            var expectedCoreMapPath = ToValidPath(contentRootPath, "App_Data", "Map");

            var hostEnvironmentMock = new Mock<IHostEnvironment>();

            hostEnvironmentMock.Setup(
                mock => mock.ContentRootPath
            ).Returns(
                contentRootPath
            );

            // When
            var serverInfo = new HostEnvironmentServerInfo(
                hostEnvironmentMock.Object
            );

            // Then
            Assert.Equal(expectedRootPath, serverInfo.RootPath);
            Assert.Equal(expectedFileSystemTempPath, serverInfo.FileSystemTempPath);
            Assert.Equal(expectedAssembliesPathPath, serverInfo.AssembliesPath);
            Assert.Equal(expectedGeneratedPath, serverInfo.GeneratedPath);
            Assert.Equal(expectedAppDataPath, serverInfo.AppDataPath);
            Assert.Equal(expectedSystemPath, serverInfo.SystemPath);
            Assert.Equal(expectedSystemBackupPath, serverInfo.SystemBackupPath);
            Assert.Equal(expectedAdminPath, serverInfo.AdminPath);
            Assert.Equal(expectedPluginsPath, serverInfo.PluginsPath);
            Assert.Equal(expectedI18nPath, serverInfo.I18nPath);
            Assert.Equal(expectedClientPath, serverInfo.ClientPath);
            Assert.Equal(expectedClientScriptsPath, serverInfo.ClientScriptsPath);
            Assert.Equal(expectedClientEntityPath, serverInfo.ClientEntityPath);
            Assert.Equal(expectedServerPath, serverInfo.ServerPath);
            Assert.Equal(expectedServerScriptsPath, serverInfo.ServerScriptsPath);
            Assert.Equal(expectedCoreMapPath, serverInfo.CoreMapPath);
        }

        private object ToValidPath(params string[] paths)
        {
            return Path.Combine(
                paths
            );
        }
    }
}