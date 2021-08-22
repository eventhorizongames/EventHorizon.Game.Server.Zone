namespace EventHorizon.Game.Server.Zone.Tests.I18n.Loader
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.I18n;
    using EventHorizon.Game.I18n.Loader;
    using EventHorizon.Game.I18n.Model;
    using EventHorizon.Game.Server.Zone.I18n.Loader;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;

    using MediatR;

    using Moq;

    using Xunit;

    public class I18nLoadHandlerTests
    {
        [Fact]
        public async Task TestShouldCallI18nRepositoryWithExpectedLocaleAndTranslationList()
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
            var localeFileFullName = $"{i18nPath}/{localeFileName}";
            var fileExtension = ".json";
            var fileList = new List<StandardFileInfo>()
            {
                new StandardFileInfo(
                    localeFileName,
                    i18nPath,
                    localeFileFullName,
                    fileExtension
                )
            };
            var localeFile = new I18nFile
            {
                Locale = expectedLocale,
                TranslationList = expectedTranslationList,
            };

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var i18nRepositoryMock = new Mock<I18nRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        i18nPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        i18nPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileList
            );
            serverInfoMock.Setup(
                mock => mock.I18nPath
            ).Returns(
                i18nPath
            );
            jsonFileLoaderMock.Setup(
                a => a.GetFile<I18nFile>(
                    localeFileFullName
                )
            ).ReturnsAsync(
                localeFile
            );

            //When
            var i18nLoadHandler = new I18nLoadHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
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
            var i18nBasePath = "/path/to/assets";
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

            var i18nPath1 = $"{i18nBasePath}/folder1";
            var localeFileName1 = "file1.json";
            var localeFileFullName1 = $"{i18nPath1}/{localeFileName1}";
            var file1Extension = ".json";
            var fileList1 = new List<StandardFileInfo>()
            {
                new StandardFileInfo(
                    localeFileName1,
                    i18nPath1,
                    localeFileFullName1,
                    file1Extension
                )
            };
            var localeFile1 = new I18nFile
            {
                Locale = expectedLocale,
                TranslationList = expectedTranslation1List,
            };
            var i18nPath2 = $"{i18nBasePath}/folder2";
            var parentFullName = i18nBasePath;
            var localeFileName2 = "file2.json";
            var localeFileFullName2 = $"{i18nPath2}/{localeFileName2}";
            var file2Extension = ".json";
            var fileList2 = new List<StandardFileInfo>()
            {
                new StandardFileInfo(
                    localeFileName2,
                    i18nPath2,
                    localeFileFullName2,
                    file2Extension
                )
            };
            var localeFile2 = new I18nFile
            {
                Locale = expectedLocale,
                TranslationList = expectedTranslation2List,
            };
            var i18nPathList = new List<StandardDirectoryInfo>
            {
                new StandardDirectoryInfo(
                    i18nPath2,
                    i18nPath2,
                    parentFullName
                )
            };

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var i18nRepositoryMock = new Mock<I18nRepository>();

            serverInfoMock.Setup(
                mock => mock.I18nPath
            ).Returns(
                i18nPath1
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        i18nPath1
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        i18nPath2
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        i18nPath1
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileList1
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfFilesFromDirectory(
                        i18nPath2
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileList2
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetListOfDirectoriesFromDirectory(
                        i18nPath1
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                i18nPathList
            );
            jsonFileLoaderMock.Setup(
                mock => mock.GetFile<I18nFile>(
                    localeFileFullName1
                )
            ).ReturnsAsync(
                localeFile1
            );
            jsonFileLoaderMock.Setup(
                mock => mock.GetFile<I18nFile>(
                    localeFileFullName2
                )
            ).ReturnsAsync(
                localeFile2
            );

            //When
            var i18nLoadHandler = new I18nLoadHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
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
