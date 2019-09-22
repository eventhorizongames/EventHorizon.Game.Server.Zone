using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n;
using EventHorizon.Game.I18n.Loader;
using EventHorizon.Game.I18n.Model;
using EventHorizon.Game.Server.Zone.I18n.Loader;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using Moq;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.I18n.Loader
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

            var i18nPath = "/path/to/assets/folder";
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
            serverInfoMock.Setup(a => a.I18nPath).Returns(i18nPath);
            var directoryResolverMock = new Mock<DirectoryResolver>();
            directoryResolverMock.Setup(
                a => a.GetFiles(
                    i18nPath
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
        [Fact]
        public async Task TestHandle_ShouldCallRepositoryMultipleTimesWhenDirectoriesAreIncluded()
        {
            //Given
            var i18nLoadEvent = new I18nLoadEvent();
            var expectedLocale = "en_US";
            var expectedTranslation1List = new Dictionary<string, string>()
            {{
                "key1", "value1"
            }};
            var expectedTranslation2List = new Dictionary<string, string>()
            {{
                "key2", "value2"
            }};

            var i18nPath1 = "/path/to/assets/folder1";
            var localeFileName1 = "file1.json";
            var fileList1 = new List<string>()
            {
                localeFileName1
            };
            var localeFile1 = new I18nFile
            {
                Locale = expectedLocale,
                TranslationList = expectedTranslation1List,
            };
            var i18nPath2 = "/path/to/assets/folder2";
            var localeFileName2 = "file2.json";
            var fileList2 = new List<string>()
            {
                localeFileName2
            };
            var localeFile2 = new I18nFile
            {
                Locale = expectedLocale,
                TranslationList = expectedTranslation2List,
            };

            var serverInfoMock = new Mock<ServerInfo>();
            serverInfoMock.Setup(a => a.I18nPath).Returns(i18nPath1);
            var directoryResolverMock = new Mock<DirectoryResolver>();
            directoryResolverMock.Setup(
                a => a.GetFiles(
                    i18nPath1
                )
            ).Returns(fileList1);
            directoryResolverMock.Setup(
                a => a.GetFiles(
                    i18nPath2
                )
            ).Returns(fileList2);
            directoryResolverMock.Setup(
                a => a.GetDirectories(
                    i18nPath1
                )
            ).Returns(
                new List<string>() {
                    i18nPath2
                }
            );
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            jsonFileLoaderMock.Setup(
                a => a.GetFile<I18nFile>(
                    localeFileName1
                )
            ).ReturnsAsync(
                localeFile1
            );
            jsonFileLoaderMock.Setup(
                a => a.GetFile<I18nFile>(
                    localeFileName2
                )
            ).ReturnsAsync(
                localeFile2
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
                    expectedTranslation1List
                )
            );
            i18nRepositoryMock.Verify(
                a => a.SetRepository(
                    expectedLocale,
                    expectedTranslation2List
                )
            );
        }
    }
}