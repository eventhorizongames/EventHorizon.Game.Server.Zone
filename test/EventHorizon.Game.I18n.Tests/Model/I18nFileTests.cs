using System.Collections.Generic;
using EventHorizon.Game.I18n.Model;
using Xunit;

namespace EventHorizon.Game.I18n.Tests.Model
{
    public class I18nFileTests
    {
        [Fact]
        public void TestWhenLocaleSetShouldReturnedExpectly()
        {
            // Given
            var expectedLocale = "en-us";

            // When
            var actual = new I18nFile
            {
                Locale = expectedLocale
            };

            // Then
            Assert.Equal(
                actual.Locale,
                expectedLocale
            );
        }
        [Fact]
        public void TestWhenTranslationListSetShouldReturnedExpectly()
        {
            // Given
            var expectedTranslationMap = new Dictionary<string, string>();

            // When
            var actual = new I18nFile
            {
                TranslationList = expectedTranslationMap
            };

            // Then
            Assert.Equal(
                actual.TranslationList,
                expectedTranslationMap
            );
        }
    }
}