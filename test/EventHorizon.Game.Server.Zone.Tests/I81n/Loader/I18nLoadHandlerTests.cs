using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n;
using EventHorizon.Game.I18n.Loader;
using EventHorizon.Game.I18n.Model;
using EventHorizon.Game.Server.Zone.External.DirectoryService;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using Moq;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.I81n.Loader
{
    public class I18nLoadHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldCallI18nRepositoryWithExpectedLocaleAndTranslationList()
        {
            //Given
            var i18nLoadEvent = new I18nLoadEvent();
            var expectedLocale = "en_US";
            var expectedTranslationList = new Dictionary<string, string>()
            {{
                "key1", "value1"
            }};

            var assetsPath = "/path/to/assets/folder";
            var localeFileName = "file.json";
            var fileList = new List<string>()
            {
                localeFileName
            };
            var localeFile = new I18nFile
            {
                Locale = expectedLocale,
                TranslationList = expectedTranslationList,
            };

            var serverInfoMock = new Mock<ServerInfo>();
            serverInfoMock.Setup(a => a.AssetsPath).Returns(assetsPath);
            var directoryResolverMock = new Mock<DirectoryResolver>();
            directoryResolverMock.Setup(
                a => a.GetFiles(
                    System.IO.Path.Combine(
                        assetsPath,
                        "I18n"
                    )
                )
            ).Returns(fileList);
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            jsonFileLoaderMock.Setup(
                a => a.GetFile<I18nFile>(
                    localeFileName
                )
            ).ReturnsAsync(
                localeFile
            );
            var i18nRepositoryMock = new Mock<I18nRepository>();

            //When
            var i18nLoadHandler = new I18nLoadHandler(
                serverInfoMock.Object,
                directoryResolverMock.Object,
                jsonFileLoaderMock.Object,
                i18nRepositoryMock.Object
            );

            await i18nLoadHandler.Handle(
                i18nLoadEvent,
                CancellationToken.None
            );

            //Then
            i18nRepositoryMock.Verify(
                a => a.SetRepository(
                    expectedLocale,
                    expectedTranslationList
                )
            );
        }
    }
}