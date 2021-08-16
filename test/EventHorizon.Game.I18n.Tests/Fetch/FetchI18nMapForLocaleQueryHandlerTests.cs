using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Game.I18n;
using EventHorizon.Game.I18n.Fetch;

using Moq;

using Xunit;

namespace EventHorizon.Game.I18n.Tests.Fetch
{
    public class FetchI18nMapForLocaleQueryHandlerTests
    {
        [Fact]
        public async Task ShouldUseDefaultLocaleWhenLocaleIsNull()
        {
            //Given
            string lookupLocale = null;
            var expectedLocale = "default";

            var i18nRepositoryMock = new Mock<I18nRepository>();

            //When
            var fetchI18nMapForLocaleQueryHandler = new FetchI18nMapForLocaleQueryHandler(
                i18nRepositoryMock.Object
            );

            await fetchI18nMapForLocaleQueryHandler.Handle(
                new FetchI18nMapForLocaleQuery(
                    lookupLocale
                ),
                CancellationToken.None
            );

            //Then
            i18nRepositoryMock.Verify(
                mock => mock.GetRepository(
                    expectedLocale
                )
            );
        }
        [Fact]
        public async Task ShouldUseDefaultLocaleWhenLookedupRepositoryIsEmpty()
        {
            //Given
            string lookupLocale = "en_US";
            var expectedLocale = "en_US";
            var expectedDefaultLocale = "default";

            var emptyLookedUpRepository = new Dictionary<string, string>();

            var i18nRepositoryMock = new Mock<I18nRepository>();
            i18nRepositoryMock.Setup(
                mock => mock.GetRepository(
                    lookupLocale
                )
            ).Returns(
                emptyLookedUpRepository
            );

            //When
            var fetchI18nMapForLocaleQueryHandler = new FetchI18nMapForLocaleQueryHandler(
                i18nRepositoryMock.Object
            );

            await fetchI18nMapForLocaleQueryHandler.Handle(
                new FetchI18nMapForLocaleQuery(
                    lookupLocale
                ),
                CancellationToken.None
            );

            //Then
            i18nRepositoryMock.Verify(
                mock => mock.GetRepository(
                    expectedLocale
                )
            );
            i18nRepositoryMock.Verify(
                mock => mock.GetRepository(
                    expectedDefaultLocale
                )
            );
        }
    }
}
