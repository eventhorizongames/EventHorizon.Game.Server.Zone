namespace EventHorizon.Zone.Core.Tests.Info
{
    using System.IO;
    using EventHorizon.Zone.Core.Info;
    using Microsoft.AspNetCore.Hosting;
    using Moq;
    using Xunit;

    public class HostingEnvironmentServerInfoTests
    {
        [Fact]
        public void TestShouldHavePathRelativePathWhenGeneatedFromHostingEnvionrment()
        {
            // Given
            var contentRootPath = "path";

            var expectedAppDataPath = ToValidPath("path", "App_Data");
            var expectedSystemPath = ToValidPath("path", "App_Data", "System");
            var expectedSystemBackupPath = ToValidPath("path", "App_Data", "__Backup__");
            var expectedAdminPath = ToValidPath("path", "App_Data", "Admin");
            var expectedPluginsPath = ToValidPath("path", "App_Data", "Plugins");
            var expectedI18nPath = ToValidPath("path", "App_Data", "I18n");
            var expectedClientPath = ToValidPath("path", "App_Data", "Client");
            var expectedClientScriptsPath = ToValidPath("path", "App_Data", "Client", "Scripts");
            var expectedClientEntityPath = ToValidPath("path", "App_Data", "Client", "Entity");
            var expectedServerPath = ToValidPath("path", "App_Data", "Server");
            var expectedServerScriptsPath = ToValidPath("path", "App_Data", "Server", "Scripts");
            var expectedCoreMapPath = ToValidPath("path", "App_Data", "Map");

            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();

            hostingEnvironmentMock.Setup(
                mock => mock.ContentRootPath
            ).Returns(
                contentRootPath
            );

            // When
            var serverInfo = new HostingEnvironmentServerInfo(
                hostingEnvironmentMock.Object
            );

            // Then
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